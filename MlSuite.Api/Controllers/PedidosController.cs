using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using MlSuite.Api.Attributes;
using MlSuite.Api.DTOs;
using MlSuite.Domain;
using MlSuite.Domain.Enums;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlApiServiceLib;
using static MlSuite.Api.Statics.StaticFuncs.Extensions;

namespace MlSuite.Api.Controllers
{
    public static class QueryFiltering
    {
        public static IQueryable<Pedido> ApplyFilters(this IQueryable<Pedido> query, FilteredQueryDto? dto)
        {
            if (dto?.Filters != null)
                foreach (var filter in dto.Filters)
                {
                    bool isForeignProperty = false;
                    string? actualProperty;
                    switch (filter.Property.ToLowerInvariant())
                    {
                        case "separado_por":
                            actualProperty = "Separação.Usuário.DisplayName";
                            isForeignProperty = true;
                            break;
                        case "num_pedido":
                            actualProperty = "Id";
                            break;
                        case "pendente":
                            actualProperty = "pendente";
                            break;
                        case "tipo_envio":
                            actualProperty = "Envio.TipoEnvio";
                            isForeignProperty = true;
                            break;
                        default:
                            actualProperty = null;
                            break;
                    }

                    if (actualProperty == null)
                        continue;

                    if (filter.Property.Equals("pendente", StringComparison.InvariantCultureIgnoreCase))
                    {
                        bool pendente = filter.Filter.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                        query = query.Where(x => (x.Separação == null) == pendente);
                        continue;
                    }
                    filter.Property = actualProperty;

                    var propertyInfo = typeof(Pedido).GetProperty(filter.Property);
                    if (propertyInfo == null && !isForeignProperty)
                    {
                        continue;
                    }

                    Type? propertyType = propertyInfo?.PropertyType;


                    switch (filter.Type.ToLowerInvariant())
                    {
                        case "igual":
                            query = query.Where($"{filter.Property} == @0", filter.Filter);
                            break;
                        case "comecando":
                            if (IsNumericType(propertyType) && !isForeignProperty)
                                continue;
                            query = query.Where($"{filter.Property}.StartsWith(@0)", filter.Filter);
                            break;
                        case "terminando":
                            if (IsNumericType(propertyType) && !isForeignProperty)
                                continue;
                            query = query.Where($"{filter.Property}.EndsWith(@0)", filter.Filter);
                            break;
                        case "maiorque":
                            if (!IsNumericType(propertyType) && !isForeignProperty)
                                continue;
                            query = query.Where($"{filter.Property} <= @0", filter.Filter);
                            break;
                        case "menorque":
                            if (!IsNumericType(propertyType) && !isForeignProperty)
                                continue;
                            query = query.Where($"{filter.Property} >= @0", filter.Filter);
                            break;
                        case "contendo":
                            if (IsNumericType(propertyType) && !isForeignProperty)
                                continue;
                            query = query.Where($"{filter.Property}.Contains(@0)", filter.Filter);
                            break;
                    }
                }

            if (dto?.OrderDtos != null)
            {
                foreach (var order in dto.OrderDtos)
                {
                    bool isForeignProperty = false;

                    if (string.IsNullOrWhiteSpace(order.Order) || string.IsNullOrWhiteSpace(order.Property))
                    {
                        continue;
                    }
                    string? actualProperty;
                    switch (order.Property.ToLowerInvariant())
                    {
                        case "separado_por":
                            actualProperty = "Separação.Usuário.DisplayName";
                            isForeignProperty = true;
                            break;
                        case "num_pedido":
                            actualProperty = "Id";
                            break;
                        case "data_abertura":
                            actualProperty = "CreatedAt";
                            break;
                        default:
                            actualProperty = null;
                            break;
                    }

                    if (actualProperty == null && !isForeignProperty)
                        continue;

                    query = order.Order.Equals("D", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy($"{actualProperty} descending")
                        : query.OrderBy($"{actualProperty}");
                }
            }

            return query;
        }

        public static IQueryable<Pedido> DefaultIncludes(this DbSet<Pedido> contextItem)
        {
            return contextItem
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.EmbalagemItem)
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Item)
                .Include(pedido => pedido.Envio)
                .ThenInclude(envio => envio.Embalagem)
                //.ThenInclude(embalagem => embalagem.EmbalagemItems)
                .Include(pedido => pedido.Separação)
                .ThenInclude(separação => separação.Separador);
        }
    }


    [Route("pedidos"), Autorizar, EnableCors]
    public class PedidosController : Controller
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public PedidosController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        [HttpPost("getPedidosFiltered"), Anônimo]
        public async Task<IActionResult> GetPedidosFiltered([FromBody] FilteredQueryDto? dto)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            IQueryable<Pedido> query = context.Pedidos
                .DefaultIncludes()
                .AsNoTracking()
                .Where(pedido => pedido.ProntoParaSeparação())
                .ApplyFilters(dto);

            int maxRecords = await query.CountAsync();
            query = query.Skip(Math.Max(dto?.Skip ?? 0, 0)).Take(Math.Min(dto?.Take ?? 15, 15));
            if (await query.CountAsync() > 0)
            {
                RetornoDto retornoDto = new RetornoDto("Pedidos encontrados", new List<PedidoDto>());
                List<MlUserAuthInfo> mlUsers = await context.MlUserAuthInfos.AsNoTracking().ToListAsync();
                List<ulong> packIdList = new();
                List<Pedido> queryResult = await query.ToListAsync();
                foreach (Pedido pedido in queryResult)
                {
                    if (pedido.PackId != null && packIdList.Contains((ulong)pedido.PackId))
                    //Se o pedido já foi processado como parte de um pack
                    {
                        continue;
                    }

                    PedidoDto pedidoEncontradoDto = new()
                    {
                        NúmPedido = pedido.Id,
                        PackId = pedido.PackId,
                        SeparadoPor = pedido.Separação?.Separador?.DisplayName,
                        TipoEnvio = pedido.Envio?.TipoEnvio ?? ShipmentType.Desconhecido,
                        MlUsername = mlUsers.First(x => x.UserId == pedido.SellerId).AccountNickname
                    };

                    if (pedido.PackId != null)
                    //Se o pedido foi parte de um pack
                    {
                        packIdList.Add((ulong)pedido.PackId); //Acrescenta à lista de pedidos processados
                        IEnumerable<Pedido> pedidosDoPack = await context.Pedidos.AsNoTracking()
                            .Where(ped => ped.PackId == pedido.PackId)
                            .Include(ped => ped.Itens)
                            .ThenInclude(item => item.Item)
                            .Include(ped => ped.Itens)
                            .ThenInclude(item => item.EmbalagemItem)
                            .ToListAsync();
                        //Obtém todos os pedidos que fazem parte do pack

                        foreach (Pedido pedidoDoPack in pedidosDoPack)
                        {
                            //Pra cada pedido que faz parte do pack, pegue os itens desse pedido e jogue no DTO
                            pedidoEncontradoDto.PedidoItems.AddRange(pedidoDoPack.Itens.Select(item => new PedidoItemDto
                            {
                                Sku = item.Sku,
                                Descrição = item.Item!.Título,
                                Quantidade = item.QuantidadeVendida,
                                UrlImagem = item.Item.PrimeiraFoto,
                                Separados = item.EmbalagemItem?.Separados ?? 0
                            }));
                        }
                    }
                    else
                    //Se o pedido não faz parte de um pack
                    {
                        //Pega os itens desse pedido e joga no DTO
                        pedidoEncontradoDto.PedidoItems.AddRange(pedido.Itens.Select(item => new PedidoItemDto
                        {
                            Sku = item.Sku,
                            Descrição = item.Item!.Título,
                            Quantidade = item.QuantidadeVendida,
                            UrlImagem = item.Item.PrimeiraFoto,
                            Separados = item.EmbalagemItem?.Separados ?? 0
                        }));
                    }

                    // Joga o DTO no objeto de retorno.
                    ((List<PedidoDto>)retornoDto.Dados!).Add(pedidoEncontradoDto);
                }

                //Retorna o DTO com o(s) registro(s) encontrados.
                return Ok(new { Registros = maxRecords, retornoDto.Mensagem, Codigo = "OK", Pedidos = retornoDto.Dados });
            }
            else
            // Caso nenhum registro tenha sido encontrado.
            {
                RetornoDto retornoDto = new RetornoDto("Nenhum dado encontrado");
                return Ok(new { Registros = maxRecords, retornoDto.Mensagem, Codigo = "OK", Pedidos = (dynamic?)null });
            }

        }

        [HttpGet("pedidoAberto"), Autorizar]
        public async Task<IActionResult> GetPedidoEmAberto()
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            //Verifica se o header veio com userInfo
            object? userUuid = HttpContext.Items["user_info"];
            if (userUuid is not UserInfo userInfo)
            {
                //Se a userInfo não vier do Header...
                var retorno1 = new RetornoDto("Uuid não foi determinada.");
                return Ok(new { retorno1.Mensagem, Codigo = "UUID_NAO_DETERMINADO" });
            }

            //Verifica se o usuário informado é valido
            UserInfo? requestingUser = await context.Usuários.FirstOrDefaultAsync(x =>
                x.Uuid == userInfo.Uuid);
            if (requestingUser == null)
            {
                //Se o usuário não tiver sido encontrado
                var retorno1 = new RetornoDto("Uuid não foi encontrada.");
                return Ok(new { retorno1.Mensagem, Codigo = "UUID_NAO_ENCONTRADO" });
            }



            var pedidoEmSeparação = await context.Pedidos
                .DefaultIncludes()
                .Where(pedido => pedido.ProntoParaSeparação())
                .FirstOrDefaultAsync(x => x.Separação != null &&
                                          x.Separação.Separador != null &&
                                          x.Separação.Separador.Uuid == requestingUser.Uuid
                                          //&& x.Itens.Any(y => y.EmbalagemItem!.Separados != y.QuantidadeVendida)
                                          );

            if (pedidoEmSeparação != null)
            {
                List<MlUserAuthInfo> mlUsers = await context.MlUserAuthInfos.AsNoTracking().ToListAsync();

                PedidoDto pedidoEmSeparaçãoDto = new PedidoDto();
                pedidoEmSeparaçãoDto.NúmPedido = pedidoEmSeparação.Id;
                pedidoEmSeparaçãoDto.SeparadoPor = pedidoEmSeparação.Separação?.Separador?.DisplayName;
                pedidoEmSeparaçãoDto.TipoEnvio = pedidoEmSeparação.Envio?.TipoEnvio ?? ShipmentType.Desconhecido;
                pedidoEmSeparaçãoDto.MlUsername = mlUsers.First(x => x.UserId == pedidoEmSeparação.SellerId).AccountNickname;


                if (pedidoEmSeparação.PackId != null)
                {
                    IEnumerable<Pedido> pedidosDoPack = await context.Pedidos
                        .Where(x => x.PackId == pedidoEmSeparação.PackId).Include(pedido => pedido.Itens)
                        .ThenInclude(pedidoItem => pedidoItem.Item).Include(pedido => pedido.Itens)
                        .ThenInclude(pedidoItem => pedidoItem.EmbalagemItem).ToListAsync();
                    foreach (Pedido pedidoDoPack in pedidosDoPack)
                    {
                        pedidoEmSeparaçãoDto.PedidoItems.AddRange(pedidoDoPack.Itens.Select(y =>
                            new PedidoItemDto
                            {
                                Sku = y.Sku,
                                Descrição = y.Item!.Título,
                                Quantidade = y.QuantidadeVendida,
                                UrlImagem = y.Item.PrimeiraFoto,
                                Separados = y.EmbalagemItem?.Separados ?? 0,

                            }));
                    }
                }
                else
                {
                    pedidoEmSeparaçãoDto.PedidoItems = pedidoEmSeparação.Itens.Select(x =>
                        new PedidoItemDto
                        {
                            Sku = x.Sku,
                            Descrição = x.Item!.Título,
                            Quantidade = x.QuantidadeVendida,
                            UrlImagem = x.Item.PrimeiraFoto,
                            Separados = x.EmbalagemItem?.Separados ?? 0

                        }).ToList();
                }

                pedidoEmSeparaçãoDto.PackId = pedidoEmSeparação.PackId;
                var retorno3 = new RetornoDto("Pedido em separação encontrado!", pedidoEmSeparaçãoDto);
                return Ok(new { retorno3.Registros, retorno3.Mensagem, Codigo = "OK", Pedidos = new PedidoDto[] { retorno3.Dados! } });
            }

            var retorno2 = new RetornoDto("Nenhum pedido em separação encontrado.");
            return Ok(new { retorno2.Mensagem, Codigo = "NENHUM_PEDIDO_EM_SEPARACAO" });
        }

        [HttpPost("processaSku"), Autorizar]
        public async Task<IActionResult> ProcessaSku([FromQuery] string sku, long numSeparacao)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            object? userUuid = HttpContext.Items["user_info"];
            if (userUuid is not UserInfo userInfo)
            {
                var retorno1 = new RetornoDto("Uuid não foi determinada.");
                return Ok(new { retorno1.Mensagem, Codigo = "UUID_NAO_DETERMINADO" });
            }

            UserInfo? requestingUser = await context.Usuários.FirstOrDefaultAsync(x =>
                x.Uuid == userInfo.Uuid);

            if (requestingUser == null)
            {
                var retorno1 = new RetornoDto("Uuid não foi encontrada.");
                return Ok(new { retorno1.Mensagem, Codigo = "UUID_NAO_ENCONTRADO" });

            }
            List<MlUserAuthInfo> mlUsers = await context.MlUserAuthInfos.AsNoTracking().ToListAsync();


            //Verifica se já há alguma separação em aberto:
            var separaçãoIniciada = await context.Separações
                .Where(x => x.Separador.Uuid == requestingUser.Uuid && x.StatusSeparação == StatusSeparação.Iniciada && x.NúmSeparação != numSeparacao)
                .FirstOrDefaultAsync();

            if (separaçãoIniciada != null)
            {
                return Ok(new { Mensagem = "Já existe uma separação iniciada. Termine antes de começar outra.", Codigo = "OUTRA_SEPARACAO_JA_INICIADA", Dados = separaçãoIniciada.NúmSeparação });
            }

            var separaçãoAberta = await context.Separações
                .Where(x => x.NúmSeparação == numSeparacao)
                .FirstOrDefaultAsync();

            if (separaçãoAberta == null)
            {
                return Ok(new { Mensagem = "A separação informada não foi encontrada.", Codigo = "SEPARACAO_NAO_ENCONTRADA" });
            }

            if (separaçãoAberta.StatusSeparação == StatusSeparação.Finalizado)
            {
                return Ok(new { Mensagem = "A separação informada já está finalizada.", Codigo = "SEPARACAO_JA_FINALIZADA" });
            }

            List<PedidoItem> pedidoItemsDaSeparação = await context.Pedidos
                .Include(x => x.Itens)
                .ThenInclude(y => y.EmbalagemItem)
                .Where(x => x.Separação != null &&
                            x.Separação.NúmSeparação == numSeparacao)
                .SelectMany(y => y.Itens)
                .ToListAsync();

            if (pedidoItemsDaSeparação.All(x => x.Sku != sku))
            {
                return Ok(new { Mensagem = "O item informado não foi encontrado.", Codigo = "ITEM_NAO_LOCALIZADO" });
            }

            if (pedidoItemsDaSeparação.Where(x => x.Sku == sku)
                .All(x => x.EmbalagemItem != null && x.EmbalagemItem.Separados == x.QuantidadeVendida))
            {
                return Ok(new { Mensagem = "O item informado foi totalmente escaneado nessa separação.", Codigo = "ITEM_JA_PROCESSADO" });
            }

            PedidoItem pedidoItemTentativo = pedidoItemsDaSeparação.First(x =>
                x.Sku == sku && (x.EmbalagemItem == null || x.EmbalagemItem.Separados < x.QuantidadeVendida));
            pedidoItemTentativo.EmbalagemItem ??= new EmbalagemItem()
            {
                Separados = 0
            };
            pedidoItemTentativo.EmbalagemItem.Separados++;

            List<Pedido> pedidosDaEmbalagem = new List<Pedido>();
            if (pedidoItemTentativo.Pedido.PackId == null)
            {
                pedidosDaEmbalagem.Add(pedidoItemTentativo.Pedido);
            }
            else
            {
                pedidosDaEmbalagem.AddRange(context.Pedidos
                    .Include(x=>x.Separação)
                    .Include(x=>x.Itens)
                    .ThenInclude(y=>y.EmbalagemItem)
                    .Where(x=>x.PackId == pedidoItemTentativo.Pedido.PackId));
            }

        }

        [HttpGet("imprimeEtiqueta"), Autorizar, EnableCors]
        public async Task<IActionResult> ImprimeEtiqueta([FromQuery] ulong? numPedido)
        {
            Response.Headers.AccessControlAllowOrigin = "*";
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            if (numPedido == null)
            {
                return BadRequest(new
                {
                    Mensagem = "O número do pedido estava em um formato incorreto",
                    Codigo = "NUM_PEDIDO_FORMATO_INCORRETO"
                });
            }

            var pedidoAImprimir = await context.Pedidos.AsNoTracking()
                .Include(x => x.Envio)
                .FirstOrDefaultAsync(x => x.Id == numPedido);

            if (pedidoAImprimir == null)
            {
                return BadRequest(new
                {
                    Mensagem = "O número do pedido não corresponde a um pedido válido",
                    Codigo = "PEDIDO_NAO_ENCONTRADO"
                });
            }

            if (pedidoAImprimir.Envio == null)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Ocorreu uma falha ao obter as informações de envio",
                    Codigo = "FALHA_INFO_ENVIO"
                });
            }

            var mlUserInfo = await context.MlUserAuthInfos.FirstOrDefaultAsync(x => x.UserId == pedidoAImprimir.SellerId);
            var mlApiService = new MlApiService(Secrets.ClientId, Secrets.SmtpPassword, Secrets.RedirectUrl);

            if (mlUserInfo == null)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Ocorreu uma falha ao obter a access key do ML",
                    Codigo = "FALHA_ACCESS_KEY_ML"
                });
            }

            var respostaEtiqueta =
                await mlApiService.GetLabelByShipment(mlUserInfo.AccessToken, pedidoAImprimir.Envio.Id.ToString());

            if (respostaEtiqueta.data == null)
            {
                return StatusCode(200, new
                {
                    Etiqueta = respostaEtiqueta.message,
                    Mensagem = respostaEtiqueta.message,
                    Codigo = "FALHA_ETIQUETA_ENVIO",
                });
            }

            string tempFolderPath = Path.Combine(Path.GetTempPath(), "MlSuite");
            Directory.CreateDirectory(tempFolderPath);

            string tempFilePath = Path.Combine(tempFolderPath, $"{pedidoAImprimir.Envio.Id}.zip");
            await System.IO.File.WriteAllBytesAsync(tempFilePath, respostaEtiqueta.data);

            string unzipPath = Path.Combine(tempFolderPath, $"Unzipped{pedidoAImprimir.Envio.Id}");
            System.IO.Compression.ZipFile.ExtractToDirectory(tempFilePath, unzipPath);

            string etiquetaFilePath = Path.Combine(unzipPath, "Etiqueta de envio.txt");
            string etiquetaContent = await System.IO.File.ReadAllTextAsync(etiquetaFilePath);

            System.IO.File.Delete(tempFilePath);
            System.IO.Directory.Delete(unzipPath, true);

            var pedidoASalvar = await context.Pedidos
                .Include(x => x.Separação)
                .FirstAsync(x => x.Id == numPedido);

            pedidoASalvar.Separação!.Etiqueta = etiquetaContent;
            context.Update(pedidoASalvar);
            await context.SaveChangesAsync();

            return Ok(new
            {
                Mensagem = "Etiqueta gerada com sucesso!",
                Etiqueta = etiquetaContent,
                Codigo = "OK"
            });
        }
    }

}
