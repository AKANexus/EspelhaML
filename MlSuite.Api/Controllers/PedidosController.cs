using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Api.Attributes;
using MlSuite.Api.DTOs;
using MlSuite.Domain;
using MlSuite.Domain.Enums;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlApiServiceLib;
using static MlSuite.Api.Statics.StaticFuncs.Extensions;

namespace MlSuite.Api.Controllers
{
    [Route("pedidos"), Autorizar, EnableCors]
    public class PedidosController : Controller
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public PedidosController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        [HttpGet("{id:long}"), Anônimo]
        public async Task<IActionResult> GetPedidoById(ulong id)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            var pedidoTentativo = await context.Pedidos.Include(pedido => pedido.Itens)
                .ThenInclude(pedidoItem => pedidoItem.Item).FirstOrDefaultAsync(x => x.Id == id);
            if (pedidoTentativo == null)
            {
                var retorno1 = new RetornoDto("Nenhum registro encontrado");
                return Ok(new { retorno1.Mensagem, Codigo = "OK" });
            }

            PedidoDto dto = new PedidoDto();
            dto.NúmPedido = pedidoTentativo.Id;
            dto.PedidoItems = pedidoTentativo.Itens.Select(x =>
                new PedidoItemDto
                {
                    Sku = x.Sku,
                    Descrição = x.Item.Título,
                    Quantidade = x.QuantidadeVendida,
                    UrlImagem = x.Item.PrimeiraFoto

                }).ToList();
            var retorno2 = new RetornoDto("Dados retornados", dto);
            return Ok(new { retorno2.Mensagem, retorno2.Registros, Codigo = "OK", Pedido = retorno2.Dados });
        }

        [HttpPost("getPedidosFiltered"), Anônimo]
        public async Task<IActionResult> GetPedidosFiltered([FromBody] FilteredQueryDto? dto)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            IQueryable<Pedido> query = context.Pedidos
                .Include(pedido => pedido.Separação)
                .ThenInclude(separação => separação!.Usuário)
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Separação)
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Item)
                .Include(pedido => pedido.Envio)
                .Where(pedido => pedido.Envio != null &&
                    (pedido.Envio.SubStatus == ShipmentSubStatus.ProntoParaColeta ||
                                 pedido.Envio.SubStatus == ShipmentSubStatus.Impresso));

            if (dto?.Filters != null)
                foreach (var filter in dto.Filters)
                {
                    var actualProperty = filter.Property.ToLowerInvariant() switch
                    {
                        "separado_por" => "Separação.Usuário.DisplayName",
                        "num_pedido" => "Id",
                        "pendente" => "pendente",
                        _ => null
                    };

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
                    if (propertyInfo == null)
                    {
                        continue;
                    }

                    Type propertyType = propertyInfo.PropertyType;


                    switch (filter.Type.ToLowerInvariant())
                    {
                        case "igual":
                            query = query.Where($"{filter.Property} == @0", filter.Filter);
                            break;
                        case "comecando":
                            if (IsNumericType(propertyType))
                                continue;
                            query = query.Where($"{filter.Property}.StartsWith(@0)", filter.Filter);
                            break;
                        case "terminando":
                            if (IsNumericType(propertyType))
                                continue;
                            query = query.Where($"{filter.Property}.EndsWith(@0)", filter.Filter);
                            break;
                        case "maiorque":
                            if (!IsNumericType(propertyType))
                                continue;
                            query = query.Where($"{filter.Property} <= @0", filter.Filter);
                            break;
                        case "menorque":
                            if (!IsNumericType(propertyType))
                                continue;
                            query = query.Where($"{filter.Property} >= @0", filter.Filter);
                            break;
                        case "contendo":
                            if (IsNumericType(propertyType))
                                continue;
                            query = query.Where($"{filter.Property}.Contains(@0)", filter.Filter);
                            break;
                    }
                }

            if (dto?.OrderDtos != null)
            {
                foreach (var order in dto.OrderDtos)
                {
                    if (string.IsNullOrWhiteSpace(order.Order) || string.IsNullOrWhiteSpace(order.Property))
                    {
                        continue;
                    }
                    var actualProperty = order.Property.ToLowerInvariant() switch
                    {
                        "separado_por" => "Separação.Usuário.DisplayName",
                        "num_pedido" => "Id",
                        "data_abertura" => "CreatedAt",
                        _ => null
                    };

                    if (actualProperty == null)
                        continue;

                    query = order.Order.Equals("D", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy($"{actualProperty} descending")
                        : query.OrderBy($"{actualProperty}");
                }
            }

            query = query.Skip(Math.Max(dto?.Skip ?? 0, 0)).Take(Math.Min(dto?.Take ?? 15, 15));
            if (await query.CountAsync() > 0)
            {
                RetornoDto retornoDto = new RetornoDto("Pedidos encontrados", new List<PedidoDto>());
                foreach (Pedido pedido in await query.ToListAsync())
                {
                    PedidoDto pedidoEncontrado = new PedidoDto();
                    pedidoEncontrado.NúmPedido = pedido.Id;
                    pedidoEncontrado.SeparadoPor = pedido.Separação?.Usuário?.DisplayName;
                    pedidoEncontrado.PedidoItems = pedido.Itens.Select(x =>
                        new PedidoItemDto
                        {
                            Sku = x.Sku,
                            Descrição = x.Item!.Título,
                            Quantidade = x.QuantidadeVendida,
                            UrlImagem = x.Item.PrimeiraFoto,
                            Separados = x.Separação?.Separados ?? 0

                        }).ToList();

                    ((List<PedidoDto>)retornoDto.Dados!).Add(pedidoEncontrado);
                }

                return Ok(new { retornoDto.Registros, retornoDto.Mensagem, Codigo = "OK", Pedidos = retornoDto.Dados });
            }
            else
            {
                RetornoDto retornoDto = new RetornoDto("Nenhum dado encontrado");
                return Ok(new { retornoDto.Registros, retornoDto.Mensagem, Codigo = "OK", Pedidos = (dynamic?)null });
            }

        }

        [HttpGet("pedidoAberto"), Autorizar]
        public async Task<IActionResult> GetPedidoEmAberto()
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

            var pedidoEmSeparação = await context.Pedidos
                .Include(pedido => pedido.Separação)
                .ThenInclude(separação => separação!.Usuário)
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Separação)
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Item)
                .Where(pedido => pedido.Envio != null &&
                                 (pedido.Envio.SubStatus == ShipmentSubStatus.ProntoParaColeta ||
                                  pedido.Envio.SubStatus == ShipmentSubStatus.Impresso))
                .FirstOrDefaultAsync(x => x.Separação != null &&
                                          x.Separação.Usuário.Uuid == requestingUser.Uuid &&
                                          x.Itens.Any(y => y.Separação!.Separados != y.QuantidadeVendida)
                                          //x.Separação.Etiqueta == null //Etiqueta já recebida
                                          );

            if (pedidoEmSeparação != null)
            {
                PedidoDto pedidoEmSeparaçãoDto = new PedidoDto();
                pedidoEmSeparaçãoDto.NúmPedido = pedidoEmSeparação.Id;
                pedidoEmSeparaçãoDto.SeparadoPor = pedidoEmSeparação.Separação?.Usuário?.DisplayName;
                pedidoEmSeparaçãoDto.PedidoItems = pedidoEmSeparação.Itens.Select(x =>
                    new PedidoItemDto
                    {
                        Sku = x.Sku,
                        Descrição = x.Item!.Título,
                        Quantidade = x.QuantidadeVendida,
                        UrlImagem = x.Item.PrimeiraFoto,
                        Separados = x.Separação?.Separados ?? 0

                    }).ToList();

                var retorno3 = new RetornoDto("Pedido em separação encontrado!", pedidoEmSeparaçãoDto);
                return Ok(new { retorno3.Registros, retorno3.Mensagem, Codigo = "OK", Pedidos = new PedidoDto[] { retorno3.Dados! } });
            }

            var retorno2 = new RetornoDto("Nenhum pedido em separação encontrado.");
            return Ok(new { retorno2.Mensagem, Codigo = "NENHUM_PEDIDO_EM_SEPARACAO" });
        }

        [HttpGet("processaSku"), Autorizar]
        public async Task<IActionResult> ProcessaSku([FromQuery] string sku)
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
            

            //Verifica se já há alguma separação em aberto:
            var pedidoEmSeparação = await context.Pedidos
                .Include(pedido => pedido.Separação)
                .ThenInclude(separação => separação!.Usuário)
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Separação)
                .Include(pedido => pedido.Itens)
                .ThenInclude(item => item.Item)
                .Where(pedido => pedido.Envio != null &&
                                 (pedido.Envio.SubStatus == ShipmentSubStatus.ProntoParaColeta ||
                                  pedido.Envio.SubStatus == ShipmentSubStatus.Impresso))
                .FirstOrDefaultAsync(x => x.Separação != null &&
                                          x.Separação.Usuário.Uuid == requestingUser.Uuid &&
                                            x.Itens.Any(y => y.Separação!.Separados != y.QuantidadeVendida
                                          //x.Separação.Etiqueta == null //Pedido está finalizado se etiqueta foi gerada

                                          ));

            if (pedidoEmSeparação != null)
            {
                PedidoDto pedidoEmSeparaçãoDto = new PedidoDto();
                pedidoEmSeparaçãoDto.NúmPedido = pedidoEmSeparação.Id;
                pedidoEmSeparaçãoDto.SeparadoPor = pedidoEmSeparação.Separação?.Usuário?.DisplayName;
                pedidoEmSeparaçãoDto.PedidoItems = pedidoEmSeparação.Itens.Select(x =>
                    new PedidoItemDto
                    {
                        Sku = x.Sku,
                        Descrição = x.Item!.Título,
                        Quantidade = x.QuantidadeVendida,
                        UrlImagem = x.Item.PrimeiraFoto,
                        Separados = x.Separação?.Separados ?? 0

                    }).ToList();

                if (pedidoEmSeparação.Itens.All(x => x.Sku != sku))
                {
                    var retorno1 = new RetornoDto("SKU informado não pertence ao pedido em separação", pedidoEmSeparaçãoDto);
                    return Ok(new
                    {
                        retorno1.Registros,
                        retorno1.Mensagem,
                        Codigo = "SKU_NAO_PERTENCE_A_PEDIDO",
                        pedidos = new PedidoDto[] { retorno1.Dados! }
                    });
                }

                var itemSeparado1 = pedidoEmSeparação.Itens.First(x => x.Sku == sku);
                itemSeparado1.Separação ??= new SeparaçãoItem();
                if (itemSeparado1.Separação.Separados == itemSeparado1.QuantidadeVendida)
                {
                    if (pedidoEmSeparação.Itens.All(x => x.Separação != null && x.QuantidadeVendida == x.Separação.Separados))
                    {
                        var retornoPedido = new RetornoDto("Esse pedido já está concluído", pedidoEmSeparaçãoDto);
                        return Ok(new
                        {
                            retornoPedido.Registros,
                            retornoPedido.Mensagem,
                            Codigo = "PEDIDO_TOTALMENTE_SEPARADO",
                            pedidos = new PedidoDto[]
                                { retornoPedido.Dados! }
                        });
                    }

                    var retornoSku = new RetornoDto("Esse SKU já foi escaneado totalmente", pedidoEmSeparaçãoDto);
                    return Ok(new
                    {
                        retornoSku.Registros,
                        retornoSku.Mensagem,
                        Codigo = "SKU_TOTALMENTE_ESCANEADO",
                        pedidos = new PedidoDto[]
                            { retornoSku.Dados! }
                    });
                }
                itemSeparado1.Separação.Separados++;

                pedidoEmSeparaçãoDto.NúmPedido = pedidoEmSeparação.Id;
                pedidoEmSeparaçãoDto.SeparadoPor = pedidoEmSeparação.Separação?.Usuário?.DisplayName;
                pedidoEmSeparaçãoDto.PedidoItems = pedidoEmSeparação.Itens.Select(x =>
                    new PedidoItemDto
                    {
                        Sku = x.Sku,
                        Descrição = x.Item!.Título,
                        Quantidade = x.QuantidadeVendida,
                        UrlImagem = x.Item.PrimeiraFoto,
                        Separados = x.Separação?.Separados ?? 0

                    }).ToList();

                context.Update(pedidoEmSeparação);
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return StatusCode(500, e);
                }

                if (pedidoEmSeparação.Itens.All(x => x.Separação != null && x.QuantidadeVendida == x.Separação.Separados))
                {
                    var retornoPedido = new RetornoDto("Esse pedido já está concluído", pedidoEmSeparaçãoDto);
                    return Ok(new
                    {
                        retornoPedido.Registros,
                        retornoPedido.Mensagem,
                        Codigo = "PEDIDO_TOTALMENTE_SEPARADO",
                        pedidos = new PedidoDto[]
                            { retornoPedido.Dados! }
                    });
                }

                var retorno2 = new RetornoDto("Pedido em separação encontrado", pedidoEmSeparaçãoDto);
                return Ok(new
                {
                    retorno2.Registros,
                    retorno2.Mensagem,
                    Codigo = "OK",
                    pedidos = new PedidoDto[]
                        { retorno2.Dados! }
                });
            }

            var pedidoTentativo = await context.Pedidos
                .Include(pedido => pedido.Itens)
                .ThenInclude(pedidoItem => pedidoItem.Item)
                .Include(pedido => pedido.Itens)
                .ThenInclude(pedidoItem => pedidoItem.Separação)
                .Where(pedido => pedido.Envio != null &&
                                 (pedido.Envio.SubStatus == ShipmentSubStatus.ProntoParaColeta ||
                                  pedido.Envio.SubStatus == ShipmentSubStatus.Impresso))
                .Where(x => x.Itens.Any(y => y.Sku == sku) && x.Separação == null)
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (pedidoTentativo == null)
            {
                var retorno1 = new RetornoDto("Nenhum pedido com o sku informado foi encontrado.");
                return Ok(new { retorno1.Mensagem, Codigo = "NENHUM_PEDIDO_ENCONTRADO" });
            }

            var itemSeparado2 = pedidoTentativo.Itens.First(x => x.Sku == sku);
            itemSeparado2.Separação ??= new SeparaçãoItem();
            if (itemSeparado2.Separação.Separados == itemSeparado2.QuantidadeVendida)
            {
                var retorno1 = new RetornoDto("Esse SKU já foi escaneado totalmente.");
                return Ok(new { retorno1.Mensagem, Codigo = "SKU_TOTALMENTE_ESCANEADO" });
            }
            itemSeparado2.Separação.Separados++;

            PedidoDto novoPedidoDto = new PedidoDto();
            novoPedidoDto.NúmPedido = pedidoTentativo.Id;
            novoPedidoDto.PedidoItems = pedidoTentativo.Itens.Select(x =>
                new PedidoItemDto
                {
                    Sku = x.Sku,
                    Descrição = x.Item.Título,
                    Quantidade = x.QuantidadeVendida,
                    UrlImagem = x.Item.PrimeiraFoto,
                    Separados = x.Separação?.Separados ?? 0

                }).ToList();
            novoPedidoDto.SeparadoPor = requestingUser.DisplayName;

            pedidoTentativo.Separação = new Separação()
            {
                Início = DateTime.Now,
                Usuário = requestingUser,
                Pedido = pedidoTentativo,
            };

            context.Update(pedidoTentativo);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            if (pedidoTentativo.Itens.All(x => x.Separação != null && x.QuantidadeVendida == x.Separação.Separados))
            {
                var retornoPedido = new RetornoDto("Esse pedido já está concluído", novoPedidoDto);
                return Ok(new
                {
                    retornoPedido.Registros,
                    retornoPedido.Mensagem,
                    Codigo = "PEDIDO_TOTALMENTE_SEPARADO",
                    pedidos = new PedidoDto[]
                        { retornoPedido.Dados! }
                });
            }

            var retorno3 = new RetornoDto("Pedido em separação iniciado", novoPedidoDto);
            return Ok(new
            {
                retorno3.Registros,
                retorno3.Mensagem,
                Codigo = "OK",
                pedidos = new PedidoDto[]
                    { retorno3.Dados! }
            });
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

        [HttpGet("imprimeEtiquetaTesting"), Autorizar, EnableCors]
        public async Task<IActionResult> ImprimeEtiquetaTesting([FromQuery] ulong? numPedido)
        {

            if (numPedido != 6969420)
            {
                return Ok(new
                {
                    Mensagem = "Este pedido não tem etiqueta a ser impressa.",
                    Codigo = "NENHUMA_ETIQUETA_ENCONTRADA"
                });
            }

            var retorno = new RetornoDto("Etiqueta gerada com sucesso",
                "^XA\n^MCY\n^CI28\n^LH5,15\n^FX  HEADER  ^FS\n^FX Logo_Meli ^FS\n^FO20,10^GFA,800,800,10,,:::::::::::O0FF,M07JFE,L07FC003FE,K07EL07E,J01EN078,J07P0E,I01CP038,I07R0E,001CK01FK038,003L0IFK0C,0078J03803CJ0E,0187J06I07I01D8,0300F00F8J0FEFE0C,02003IFK01J06,04I01C6P02,08K0401FM01,1L08060CM083K0100C02M0C2M01001M046K0306I0CL064K0198I02L024Q01L02CR08K03CR04K03FR02K03FFQ01J07!C1FQ0C007E3C03EP0203F03C0078O010F003CI0EF1N0F8003CI070C4M06I03CI02003CL02I03CI02P02I036I03N0106I066I01J08J0C4I067J0EI08J078I0E38I03I0E00406I01C3CI01800100204I01C3CJ0FI080118I03C1EJ03800801FJ0780FK0C008018J0F,078J07C0823J01F,07EJ01C1C36J07E,03FK031C3K0FC,01FCJ01E18J01F8,00FER07F,007F8P01FE,003FFP0FFC,I0FFEN07FF,I03FFCL03FFC,J0IFCJ03IF,J07PFE,K0PF,K01NF8,L01LF8,N0JF,,:::::::::::^FS\n^FO120,20^A0N,24,24^FH^FDDigitron Distribuidora Tecnologicas Ltda ^FS\n^FO120,43^A0N,24,24^FH^FB550,2,0,L^FDAv_2E Pref_2E Jo_C3_A3o Vila_2DLobos Quero 1560, Jardim Itaquiti^FS\n^FO120,90^A0N,24,24^FH^FB550,1,0,L^FDBarueri, BR-SP - 06422122^FS\n^FO120,120^A0N,24,24^FDPack ID: 20000^FS\n^FO272,117^A0N,27,27^FD04602471243^FS\n^FX LAST CLUSTER  ^FS\n^FO20,150^GB210,45,45^FS\n^FO20,156^A0N,45,45^FB210,1,0,C^FR^FDXSP1^FS\n^FX END LAST CLUSTER  ^FS\n^FO480,150^GB330,40,40^FS\n^FO410,160^A0N,22,22^FB460,1,0,C^FR^FH^FDENTREGAR NA COLETA^FS\n^FX  Shipment_Number_Bar_Code  ^FS\n^FO230,210^BY3,,0^BCN,160,N,N,N^FD>:42413378328^FS\n^FO95,385^A0N,30,30^FB390,1,0,R^FD424133^FS\n^FO488,381^A0N,35,35^FB400,1,0,L^FD78328^FS\n^FX  END_HEADER  ^FS\n^FX  CUSTOM_DATA  ^FS\n^FO0,580^A0N,175,175^FB630,1,0,R^FDSSP22^FS\n^FO670,640^A0N,47,47^FB200,1,0,L^FD02:00^FS\n^FO0,790^A0N,28,28^FB533,1,0,R^FDXSP1 > SSP22 > ^FS\n^FO538,785^A0N,40,40^FD2^FS\n^FO0,830^A0N,38,38^FB820,1,0,C^FDSEX 07/07/2023   CEP: 13570080   NF: 6324^FS\n^FX  END CUSTOM_DATA  ^FS\n^FO0,950^GB850,0,2^FS\n^FX  RECEIVER ZONE  ^FS\n^FO30,970^A0N,26,26^FB600,2,0,L^FH^FDVal_C3_A9ria Boa Sorte Hernandez Martins (BSRADM1)^FS\n^FO30,1030^A0N,26,26^FB600,2,0,L^FH^FDEndere_C3_A7o: Rua Jos_C3_A9 Barnab_C3_A9 400_2c Jardim Ricetti^FS\n^FO30,1090^A0N,30,30^FDCEP: 13570080^FS\n^FO30,1089^A0N,30,30^FDCEP: 13570080^FS\n^FO30,1121^A0N,26,26^FB600,2,1000,L^FH^FDCidade de destino: S_C3_A3o Carlos_2c S_C3_A3o Paulo^FS\n^FO30,1150^A0N,26,26^FB600,5,0,L^FH^FDComplemento: Referencia: Proximo a Padaria Nosso P_C3_A3o^FS\n^FX  QR Code  ^FS\n^FO650,985^BY2,2,0^BQN,2,5^FDLA,{\"id\":\"42413378328\",\"t\":\"lm\"}^FS\n^FO650,1130^GB105,40,40^FS\n^FO650,1135^A0N,35,35^FB105,1,0,C^FR^FDR^FS\n^FX  END_FOOTER  ^FS\n^XZ");
            return Ok(new { retorno.Mensagem, Etiqueta = retorno.Dados, Codigo = "OK" });
        }
    }
}
