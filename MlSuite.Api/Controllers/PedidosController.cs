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
// ReSharper disable StringLiteralTypo

namespace MlSuite.Api.Controllers
{
    public static class QueryFiltering
    {
        public static IQueryable<Order> ApplyFilters(this IQueryable<Order> query, FilteredQueryDto? dto)
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
                        //bool pendente = filter.Filter.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                        //query = query.Where(x => (x.Separação == null) == pendente);
                        continue;
                    }
                    filter.Property = actualProperty;

                    var propertyInfo = typeof(Order).GetProperty(filter.Property);
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
    }


    [Route("pedidos"), Autorizar, EnableCors]
    public class PedidosController : Controller
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public PedidosController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        
        [HttpGet("criaSeparacao"), Anônimo]
        public async Task<IActionResult> CriaSeparação([FromQuery] string pedidos)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            ulong[] pedidoIds = pedidos.Split(',').Select(ulong.Parse).ToArray();
            Order[] tentativos = await context.Pedidos
                .Include(x=>x.Envio)
                .Include(x=>x.Itens)
                .ThenInclude(y=>y.Item)
                .Where(pedido => pedido.Envio != null &&
                                 pedido.Envio.Status == ShipmentStatus.ProntoParaEnvio &&
                                 pedido.Envio.SubStatusDescrição != "invoice_pending" &&
                                 pedido.Envio.SubStatusDescrição != "picked_up" &&
                                 pedido.Envio.TipoEnvio != ShipmentType.Fulfillment &&
                                 (pedido.Envio.SubStatus == ShipmentSubStatus.ProntoParaColeta ||
                                  pedido.Envio.SubStatus == ShipmentSubStatus.Impresso))
                .Where(x => pedidoIds.Contains(x.Pack.Id) || pedidoIds.Contains(x.Id))
                .ToArrayAsync();
            List<ulong> pedidosInválidos = new();
            Separação novaSeparação = new()
            {
                Usuário = await context.Usuários.FirstAsync(x=>x.Username == "Teste"),
                Identificador = (ulong)DateTime.UtcNow.Ticks
                
            };
            foreach (Order tentativo in tentativos)
            {
                var separaçãoTentativo = await context.Separações
                    .Include(x=>x.Embalagens)
                    .FirstOrDefaultAsync(x => x.Embalagens.Any(y =>
                        y.ReferenciaId == tentativo.Id && y.TipoVendaMl == TipoVendaMl.Order));

                if (separaçãoTentativo != null)
                {
                    pedidosInválidos.Add(tentativo.Id);
                }
                else
                {
                    novaSeparação.Embalagens.Add(new Embalagem()
                    {
                        TipoVendaMl = TipoVendaMl.Order,
                        ShippingId = tentativo.Envio!.Id,
                        ReferenciaId = tentativo.Id,
                        EmbalagemItems = tentativo.Itens.Select(y=>new EmbalagemItem()
                        {
                            SKU = y.Sku,
                            ImageUrl = y.Item!.PrimeiraFoto,
                            QuantidadeEscaneada = 0,
                            QuantidadeAEscanear = y.QuantidadeVendida,
                            Descrição = y.Título
                        }).ToList()
                    });
                }
            }

            if (novaSeparação.Embalagens.Count > 0)
            {
                context.Update(novaSeparação);
                await context.SaveChangesAsync();
            }

            return Ok(new RetornoDto(mensagem: "Separação criada com sucesso!",
                new { num_separacao = novaSeparação.Identificador, pedidos_inválidos = pedidosInválidos }));
        }

        [HttpPost("getSeparacoes"), Anônimo]
        public async Task<IActionResult> GetSeparações()
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            var separaçõesNaoDesignadas = await context.Separações
                .Where(x => x.Usuário == null)
                .ToListAsync();

            if (separaçõesNaoDesignadas.Count > 0)
            {
                var r1 = new RetornoDto("Foram encontradas separações", separaçõesNaoDesignadas);
                return Ok(r1);
            }

            var r2 = new RetornoDto("Não há separações pendentes");
            return Ok(r2);
        }

        [HttpGet("separacaoAberta"), Autorizar]
        public async Task<IActionResult> GetSeparaçãoEmAberto()
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

            var separaçãoAberta = await context.Separações
                .Include(x=>x.Embalagens)
                .ThenInclude(y=>y.EmbalagemItems)
                .FirstOrDefaultAsync(separação =>
                    separação.Usuário == requestingUser &&
                    separação.Embalagens.Any(x=>x.StatusEmbalagem != StatusEmbalagem.Impresso));
            if (separaçãoAberta != null)
            {
                var r1 = new RetornoDto("Há uma separação em aberto", separaçãoAberta);
                return Ok(r1);
            }

            var r2 = new RetornoDto("Não há separações em aberto");
            return Ok(r2);
        }

        [HttpPost("processaSku"), Autorizar]
        public async Task<IActionResult> ProcessaSku([FromQuery] string sku, [FromQuery] ulong idSeparacao)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            object? userUuid = HttpContext.Items["user_info"];
            if (userUuid is not UserInfo userInfo)
            {
                var r1 = new RetornoDto("Uuid não foi determinada.");
                return Ok(new { r1.Mensagem, Codigo = "UUID_NAO_DETERMINADO" });
            }

            UserInfo? requestingUser = await context.Usuários.FirstOrDefaultAsync(x =>
                x.Uuid == userInfo.Uuid);

            if (requestingUser == null)
            {
                var r2 = new RetornoDto("Uuid não foi encontrada.");
                return Ok(new { r2.Mensagem, Codigo = "UUID_NAO_ENCONTRADO" });

            }

            var workingSeparação = await context.Separações
                .Include(x => x.Embalagens)
                .ThenInclude(y => y.EmbalagemItems)
                .Include(x=>x.Usuário)
                .FirstOrDefaultAsync(separação => separação.Identificador == idSeparacao);


            if (workingSeparação == null)
            {
                var r4 = new RetornoDto("ID separação inválido", null, "SEPARACAO_NAO_ENCONTRADA");
                return Ok(r4);
            }

            if (workingSeparação.Usuário != requestingUser)
            {
                var r4 = new RetornoDto("Essa separação está sendo tratada por outro usuário", null, "SEPARACAO_DE_OUTRO_USUARIO");
                return Ok(r4);
            }

            var separaçãoEmAberto = await context.Separações
                .Include(x => x.Embalagens)
                .Include(x => x.Usuário)
                .FirstOrDefaultAsync(separação =>
                    separação.Usuário == requestingUser &&
                    separação.Embalagens.Any(x => x.StatusEmbalagem != StatusEmbalagem.Impresso) &&
                    separação.Identificador != idSeparacao);

            if (separaçãoEmAberto != null)
            {
                var r3 = new RetornoDto("Usuário já está com outra separação em aberto", separaçãoEmAberto, "SEPARACAO_EM_ANDAMENTO");
                return Ok(r3);
            }


            var workingEmbalagem =
                workingSeparação.Embalagens.FirstOrDefault(x => x.StatusEmbalagem == StatusEmbalagem.Iniciado);
            if (workingEmbalagem != null)
            {
                //Verificar se o SKU está nessa embalagem
                if (workingEmbalagem.EmbalagemItems.All(x => x.SKU != sku))
                {
                    var r5 = new RetornoDto("O SKU informado não pertence a essa embalagem", null,
                        "SKU_NAO_ENCONTRADO");
                    return Ok(r5);
                }

                if (workingEmbalagem.EmbalagemItems.Where(x => x.SKU == sku)
                    .All(x => x.QuantidadeEscaneada == x.QuantidadeAEscanear))
                {
                    var r6 = new RetornoDto("A quantidade necessária desse item já foi escaneada", null,
                        "QUANTIDADE_MAXIMA_ATINGIDA");
                    return Ok(r6);
                }

                var embalagemItemTentativo = workingEmbalagem.EmbalagemItems.First(x =>
                    x.SKU == sku && x.QuantidadeEscaneada < x.QuantidadeAEscanear);

                embalagemItemTentativo.QuantidadeEscaneada++;
            }

            else
            {
                //Verificar se o SKU digitado pertence a alguma embalagem da separação
                workingEmbalagem =
                    workingSeparação.Embalagens.FirstOrDefault(x => x.EmbalagemItems.Any(y => y.SKU == sku));

                if (workingEmbalagem == null)
                {
                    var r7 = new RetornoDto("O sku informado não pertence a essa embalagem");
                    return Ok(r7);
                }

                workingEmbalagem.EmbalagemItems.First(x => x.SKU == sku).QuantidadeEscaneada++;
                workingEmbalagem.StatusEmbalagem = StatusEmbalagem.Aberto;
            }
            
            //Verificar se a embalagem finalizou
            if (workingEmbalagem.EmbalagemItems.All(x => x.QuantidadeEscaneada == x.QuantidadeAEscanear))
            {
                //Embalagem finalizou

                //Obtém etiqueta
                var mlUserInfo = await context.MlUserAuthInfos.FirstOrDefaultAsync(x => x.UserId == workingSeparação.SellerId);
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
                    await mlApiService.GetLabelByShipment(mlUserInfo.AccessToken, workingEmbalagem.ShippingId.ToString());

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

                string tempFilePath = Path.Combine(tempFolderPath, $"{workingEmbalagem.ShippingId}.zip");
                await System.IO.File.WriteAllBytesAsync(tempFilePath, respostaEtiqueta.data);

                string unzipPath = Path.Combine(tempFolderPath, $"Unzipped{workingEmbalagem.ShippingId}");
                System.IO.Compression.ZipFile.ExtractToDirectory(tempFilePath, unzipPath);

                string etiquetaFilePath = Path.Combine(unzipPath, "Etiqueta de envio.txt");
                string etiquetaContent = await System.IO.File.ReadAllTextAsync(etiquetaFilePath);

                System.IO.File.Delete(tempFilePath);
                System.IO.Directory.Delete(unzipPath, true);

                //Grava etiqueta na Embalagem
                workingEmbalagem.Etiqueta = etiquetaContent;

                //Altera status embalagem para "Impresso"
                workingEmbalagem.StatusEmbalagem = StatusEmbalagem.Impresso;
                
                //Retorna embalagem com etiqueta
                var r8 = new RetornoDto("Embalagem finalizada com sucesso!", workingEmbalagem);
                return Ok(r8);
            }

            //Se não
            else
            {
                //Embalagem ainda não finalizou
                //Retorna "Item adicionado com sucesso" + embalagem
                var r9 = new RetornoDto("Embalagem em processo de separação", workingEmbalagem);
                return Ok(r9);
            }
            
        }

        [HttpGet("verificaSeparacao"), Autorizar]
        public async Task<IActionResult> VerificaSeparação([FromQuery] ulong idSeparacao)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            object? userUuid = HttpContext.Items["user_info"];
            if (userUuid is not UserInfo userInfo)
            {
                var r1 = new RetornoDto("Uuid não foi determinada.");
                return Ok(new { r1.Mensagem, Codigo = "UUID_NAO_DETERMINADO" });
            }

            UserInfo? requestingUser = await context.Usuários.FirstOrDefaultAsync(x =>
                x.Uuid == userInfo.Uuid);

            if (requestingUser == null)
            {
                var r2 = new RetornoDto("Uuid não foi encontrada.");
                return Ok(new { r2.Mensagem, Codigo = "UUID_NAO_ENCONTRADO" });

            }

            var workingSeparação = await context.Separações
                .Include(x => x.Embalagens)
                .ThenInclude(y => y.EmbalagemItems)
                .Include(x=>x.Usuário)
                .FirstOrDefaultAsync(separação => separação.Identificador == idSeparacao);


            if (workingSeparação == null)
            {
                var r4 = new RetornoDto("ID separação inválido", null, "SEPARACAO_NAO_ENCONTRADA");
                return Ok(r4);
            }

            if (workingSeparação.Usuário != requestingUser)
            {
                var r4 = new RetornoDto("Essa separação está sendo tratada por outro usuário", null, "SEPARACAO_DE_OUTRO_USUARIO");
                return Ok(r4);
            }

            var separaçãoEmAberto = await context.Separações
                .Include(x => x.Embalagens)
                .Include(x => x.Usuário)
                .FirstOrDefaultAsync(separação =>
                    separação.Usuário == requestingUser &&
                    separação.Embalagens.Any(x => x.StatusEmbalagem != StatusEmbalagem.Impresso) &&
                    separação.Identificador != idSeparacao);

            if (separaçãoEmAberto != null)
            {
                var r3 = new RetornoDto("Usuário já está com outra separação em aberto", separaçãoEmAberto, "SEPARACAO_EM_ANDAMENTO");
                return Ok(r3);
            }

            var r5 = new RetornoDto("Separação pronta para ser embalada", workingSeparação);
            return Ok(r5);
        }
    }
}
