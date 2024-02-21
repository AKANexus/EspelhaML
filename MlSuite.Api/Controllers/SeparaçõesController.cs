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
                            actualProperty = "Shipping.TipoEnvio";
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



    [Route("separacoes"), Autorizar, EnableCors]
    public class SeparaçõesController : Controller
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public SeparaçõesController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        //[HttpGet("testeCriar"), Anônimo]
        //public async Task<IActionResult> CriaSeparaçãoDeTeste([FromQuery] string pedidos)
        //{
        //    var provider = _scopeFactory.CreateScope().ServiceProvider;
        //    var context = provider.GetRequiredService<TrilhaDbContext>();

        //    ulong[] pedidoIds = pedidos.Split(',').Select(ulong.Parse).ToArray();
        //    Order[] tentativos = await context.Orders
        //        .Include(x => x.Shipping)
        //        .Include(x => x.Itens)
        //        .ThenInclude(y => y.Item)
        //        .Where(pedido => pedido.Shipping != null &&
        //                         pedido.Shipping.Status == ShipmentStatus.ProntoParaEnvio &&
        //                         pedido.Shipping.SubStatusDescrição != "invoice_pending" &&
        //                         pedido.Shipping.SubStatusDescrição != "picked_up" &&
        //                         pedido.Shipping.TipoEnvio != ShipmentType.Fulfillment &&
        //                         (pedido.Shipping.SubStatus == ShipmentSubStatus.ProntoParaColeta ||
        //                          pedido.Shipping.SubStatus == ShipmentSubStatus.Finalizado))
        //        .Where(x => pedidoIds.Contains(x.Pack.Id) || pedidoIds.Contains(x.Id))
        //        .ToArrayAsync();
        //    List<ulong> pedidosInválidos = new();
        //    Separação novaSeparação = new()
        //    {
        //        Usuário = await context.Usuários.FirstAsync(x => x.Username == "Teste"),
        //        Identificador = (long)DateTime.UtcNow.Ticks

        //    };
        //    foreach (Order tentativo in tentativos)
        //    {
        //        var separaçãoTentativo = await context.Separações
        //            .Include(x => x.Embalagens)
        //            .FirstOrDefaultAsync(x => x.Embalagens.Any(y =>
        //                y.ReferenciaId == tentativo.Id && y.TipoVendaMl == TipoVendaMl.Order));

        //        if (separaçãoTentativo != null)
        //        {
        //            pedidosInválidos.Add(tentativo.Id);
        //            //continue;
        //        }
        //        else
        //        {
        //            if (novaSeparação.SellerId == default)
        //                novaSeparação.SellerId = tentativo.SellerId;
        //            else
        //            {
        //                if (tentativo.SellerId != novaSeparação.SellerId)
        //                {
        //                    pedidosInválidos.Add(tentativo.Id);
        //                    continue;
        //                }
        //            }
        //            novaSeparação.Embalagens.Add(new Embalagem()
        //            {
        //                TipoVendaMl = TipoVendaMl.Order,
        //                Shipping = tentativo.Shipping,
        //                ReferenciaId = tentativo.Id,
        //                EmbalagemItems = tentativo.Itens.Select(y => new EmbalagemItem()
        //                {
        //                    SKU = y.Sku,
        //                    ImageUrl = y.Item!.PrimeiraFoto,
        //                    QuantidadeEscaneada = 0,
        //                    QuantidadeAEscanear = y.QuantidadeVendida,
        //                    Descrição = y.Título
        //                }).ToList()
        //            });
        //        }
        //    }

        //    if (novaSeparação.Embalagens.Count > 0)
        //    {
        //        context.Update(novaSeparação);
        //        await context.SaveChangesAsync();
        //    }

        //    return Ok(new RetornoDto(mensagem: "Separação criada com sucesso!",
        //        new { num_separacao = novaSeparação.Identificador, pedidos_inválidos = pedidosInválidos }));
        //}

        [HttpPost("criar"), Autorizar]
        public async Task<IActionResult> CriaSeparação([FromBody] CriarSeparaçãoRootDto dto)
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

            //List<Order> orders = new List<Order>();
            //List<Pack> packs = new List<Pack>();
            Separação novaSeparação = new()
            {
                Embalagens = new List<Embalagem>()
            };
            ulong? selectedSellerId = null;


            foreach (CriarSeparaçãoItemDto itemDto in dto.Pedidos)
            {
                if (itemDto.Tipo[..1].Equals("P", StringComparison.InvariantCultureIgnoreCase))
                {
                    var packTentativo = await context.Packs

                        .Include(pack => pack.Pedidos)
                        .ThenInclude(order => order.Itens)
                        .ThenInclude(orderItem => orderItem.Item)

                        .Include(pack => pack.Shipping)

                        .FirstOrDefaultAsync(x => x.Id == itemDto.Id);

                    if (packTentativo == null)
                    {
                        var r1 = new RetornoDto("Um ou mais pedidos não foram encontrados no sistema", itemDto,
                            "PEDIDO_NAO_ENCONTRADO");
                        return BadRequest(r1);
                    }
                    else
                    {
                        selectedSellerId ??= packTentativo.Pedidos?.First().SellerId;
                        if (selectedSellerId != packTentativo.Pedidos?.First().SellerId)
                        {
                            var r2 = new RetornoDto("Uma separação pode ter pedidos de apenas uma loja", null,
                                "MULTIPLAS_STOREFRONTS");
                            return BadRequest(r2);
                        }

                        var novaEmbalagem = new Embalagem()
                        {
                            ReferenciaId = packTentativo.Id,
                            TipoVendaMl = TipoVendaMl.Pack,
                            StatusEmbalagem = StatusEmbalagem.Designado,
                            Shipping = packTentativo.Shipping,
                            EmbalagemItems = packTentativo.Pedidos.SelectMany(x => x.Itens.Select(y => new EmbalagemItem()
                            {
                                QuantidadeEscaneada = 0,
                                QuantidadeAEscanear = y.QuantidadeVendida,
                                Descrição = y.Título,
                                SKU = y.Sku,
                                ImageUrl = y.Item.PrimeiraFoto
                            })).ToList()
                        };
                        novaSeparação.Embalagens.Add(novaEmbalagem);
                    }
                }
                else if (itemDto.Tipo[..1].Equals("O", StringComparison.InvariantCultureIgnoreCase))
                {
                    var orderTentativa = await context.Orders

                        .Include(order => order.Itens)
                        .ThenInclude(orderItem => orderItem.Item)

                        .Include(pack => pack.Shipping)

                        .FirstOrDefaultAsync(x => x.Id == itemDto.Id);

                    if (orderTentativa == null)
                    {
                        var r1 = new RetornoDto("Um ou mais pedidos não foram encontrados no sistema", itemDto,
                            "PEDIDO_NAO_ENCONTRADO");
                        return BadRequest(r1);
                    }
                    else
                    {
                        selectedSellerId ??= orderTentativa.SellerId;
                        if (selectedSellerId != orderTentativa.SellerId)
                        {
                            var r2 = new RetornoDto("Uma separação pode ter pedidos de apenas uma loja", null,
                                "MULTIPLAS_STOREFRONTS");
                            return BadRequest(r2);
                        }

                        var novaEmbalagem = new Embalagem()
                        {
                            ReferenciaId = orderTentativa.Id,
                            TipoVendaMl = TipoVendaMl.Order,
                            StatusEmbalagem = StatusEmbalagem.Designado,
                            Shipping = orderTentativa.Shipping,
                            EmbalagemItems = orderTentativa.Itens.Select(y => new EmbalagemItem()
                            {
                                QuantidadeEscaneada = 0,
                                QuantidadeAEscanear = y.QuantidadeVendida,
                                Descrição = y.Título,
                                SKU = y.Sku,
                                ImageUrl = y.Item.PrimeiraFoto
                            }).ToList()
                        };
                        novaSeparação.Embalagens.Add(novaEmbalagem);
                    }
                }
                novaSeparação.SellerId = (ulong)selectedSellerId;
            }

            context.Add(novaSeparação);
            await context.SaveChangesAsync();

            var r0 = new RetornoDto("Separação gerada com sucesso", novaSeparação);
            return Ok(r0);
        }

        [HttpPost("assumir"), Autorizar]
        public async Task<IActionResult> AssumirSeparação(long separação)
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

            var separaçãoTentativa = await context.Separações
                .Include(separação => separação.Usuário)
                .FirstOrDefaultAsync(x => x.Identificador == separação);

            if (separaçãoTentativa == null)
            {
                var retorno1 = new RetornoDto("Separação não foi encontrada.");
                return Ok(new { retorno1.Mensagem, Codigo = "SEPARACAO_NAO_ENCONTRADO" });
            }

            if (separaçãoTentativa.Usuário != null)
            {
                var retorno1 = new RetornoDto("Separação já assumida.");
                return Ok(new { retorno1.Mensagem, Codigo = "SEPARACAO_JA_ASSUMIDA" });
            }

            //separaçãoTentativa.Início = DateTime.UtcNow;
            separaçãoTentativa.Usuário = requestingUser;

            context.Update(separaçãoTentativa);
            await context.SaveChangesAsync();

            var retorno2 = new RetornoDto("Separação assumida com sucesso",
                new { Usuário = requestingUser.DisplayName, separaçãoTentativa.Identificador });
            return Ok(retorno2);

        }

        [HttpGet(""), Autorizar]
        public async Task<IActionResult> GetSeparações([FromQuery] string assumidas = "N")
        {
            if (!new[] { "S", "N", "s", "n" }.Contains(assumidas))
            {
                var retorno0 = new RetornoDto("Valor inválido para a consulta \"assumidas\"", null, "FILTRO_INVALIDO");
                return BadRequest(retorno0);
            }
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

            IQueryable<Separação> query = context.Separações
                .Include(x => x.Embalagens)
                .ThenInclude(y => y.EmbalagemItems)
                .Include(x=>x.Usuário);

            if (assumidas.Equals("n", StringComparison.InvariantCultureIgnoreCase))
            {
                query = query.Where(x => x.Usuário == null);
            }
            else
            {
                query = query.Where(x =>x.Usuário != null && x.Usuário.Uuid == requestingUser.Uuid && x.Fim != null);
            }


            var separaçõesARetornar = await query.ToListAsync();

            if (separaçõesARetornar.Count > 0)
            {
                Dictionary<ulong, MlUserAuthInfo> sellers = await context.MlUserAuthInfos.ToDictionaryAsync(x => x.UserId);

                var r1 = new RetornoDto("Foram encontradas separações", separaçõesARetornar.Select(x=> new SeparaçãoDto(x, sellers[x.SellerId])));
                return Ok(r1);
            }

            var r2 = new RetornoDto("Não há separações pendentes");
            return Ok(r2);
        }

        [HttpGet("aberta"), Autorizar]
        public async Task<IActionResult> GetSeparaçãoEmAndamento()
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
                .Include(x => x.Embalagens)
                .ThenInclude(y => y.EmbalagemItems)
                .FirstOrDefaultAsync(separação =>
                    separação.Usuário == requestingUser &&
                    separação.Embalagens.Any(x => x.StatusEmbalagem != StatusEmbalagem.Finalizado));
            if (separaçãoAberta != null)
            {
                var r1 = new RetornoDto("Há uma separação em aberto", separaçãoAberta);
                return Ok(r1);
            }

            var r2 = new RetornoDto("Não há separações em aberto");
            return Ok(r2);
        }

        [HttpPost("processar"), Autorizar]
        public async Task<IActionResult> ProcessaSku([FromQuery] string sku, [FromQuery] long? idSeparacao = null)
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

            Separação? workingSeparação = null;
            
            if (idSeparacao is null)
            {
                workingSeparação = await context.Separações
                    .Include(x => x.Embalagens)
                    .ThenInclude(y => y.EmbalagemItems)
                    .Include(x => x.Usuário).Include(separação => separação.Embalagens)
                    .ThenInclude(embalagem => embalagem.Shipping)
                    .FirstOrDefaultAsync(x=>x.Embalagens.All(y=>y.StatusEmbalagem == StatusEmbalagem.Designado &&
                                                                x.Usuário != null &&
                                                                x.Usuário.Uuid == requestingUser.Uuid &&
                                                                x.Embalagens.Any(embalagem => 
                                                                    embalagem.EmbalagemItems.Any(embalagemItem => 
                                                                        embalagemItem.SKU == sku))));
            }

            else
            {
                workingSeparação = await context.Separações
                    .Include(x => x.Embalagens)
                    .ThenInclude(y => y.EmbalagemItems)
                    .Include(x => x.Usuário).Include(separação => separação.Embalagens)
                    .ThenInclude(embalagem => embalagem.Shipping)
                    .FirstOrDefaultAsync(separação => separação.Identificador == idSeparacao && separação.Usuário != null &&
                                                      separação.Usuário.Uuid == requestingUser.Uuid);
            }


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
                    separação.Início != null &&
                    separação.Fim == null &&
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
                    workingSeparação.Embalagens.FirstOrDefault(x => x.EmbalagemItems
                        .Where(y=>y.QuantidadeAEscanear != y.QuantidadeEscaneada)
                        .Any(y => y.SKU == sku));

                if (workingEmbalagem == null)
                {
                    var r7 = new RetornoDto("O sku informado não pertence a essa separação");
                    return Ok(r7);
                }

                workingEmbalagem.EmbalagemItems.First(x => x.SKU == sku).QuantidadeEscaneada++;
                workingEmbalagem.StatusEmbalagem = StatusEmbalagem.Iniciado;
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
                    await mlApiService.GetLabelByShipment(mlUserInfo.AccessToken, workingEmbalagem.Shipping.Id.ToString());

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

                string tempFilePath = Path.Combine(tempFolderPath, $"{workingEmbalagem.Shipping.Id}.zip");
                await System.IO.File.WriteAllBytesAsync(tempFilePath, respostaEtiqueta.data);

                string unzipPath = Path.Combine(tempFolderPath, $"Unzipped{workingEmbalagem.Shipping.Id}");
                System.IO.Compression.ZipFile.ExtractToDirectory(tempFilePath, unzipPath);

                string etiquetaFilePath = Path.Combine(unzipPath, "Etiqueta de envio.txt");
                string etiquetaContent = await System.IO.File.ReadAllTextAsync(etiquetaFilePath);

                System.IO.File.Delete(tempFilePath);
                System.IO.Directory.Delete(unzipPath, true);

                //Grava etiqueta na Embalagem
                workingEmbalagem.Etiqueta = etiquetaContent;
                workingEmbalagem.TimestampImpressao = DateTime.UtcNow;

                //Grava etiqueta no shipping
                switch (workingEmbalagem.TipoVendaMl)
                {
                    case TipoVendaMl.Pack:
                        var packTentativo = await context.Packs.Include(x => x.Shipping)
                            .FirstAsync(x => x.Id == workingEmbalagem.ReferenciaId);
                        packTentativo.Shipping.Etiqueta = etiquetaContent;
                        context.Update(packTentativo);
                        break;
                    case TipoVendaMl.Order:
                        var orderTentativa = await context.Orders.Include(x => x.Shipping)
                            .FirstAsync(x => x.Id == workingEmbalagem.ReferenciaId);
                        orderTentativa.Shipping.Etiqueta = etiquetaContent;
                        context.Update(orderTentativa);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                //Altera status embalagem para "Finalizado"
                workingEmbalagem.StatusEmbalagem = StatusEmbalagem.Finalizado;

                context.Update(workingEmbalagem);
                await context.SaveChangesAsync();

                Dictionary<ulong, MlUserAuthInfo> sellers = await context.MlUserAuthInfos.ToDictionaryAsync(x => x.UserId);

                //Retorna embalagem com etiqueta
                var r8 = new RetornoDto("Embalagem finalizada com sucesso!", new SeparaçãoDto(workingSeparação, sellers[workingSeparação.SellerId]));
                return Ok(r8);
            }

            //Se não
            else
            {
                //Embalagem ainda não finalizou
                //Retorna "Item adicionado com sucesso" + embalagem

                context.Update(workingEmbalagem);
                await context.SaveChangesAsync();

                Dictionary<ulong, MlUserAuthInfo> sellers = await context.MlUserAuthInfos.ToDictionaryAsync(x => x.UserId);

                var r9 = new RetornoDto("Embalagem em processo de separação", new SeparaçãoDto(workingSeparação, sellers[workingSeparação.SellerId]));
                return Ok(r9);
            }

        }

        [HttpGet("verificar"), Autorizar]
        public async Task<IActionResult> VerificaSeparação([FromQuery] long idSeparacao)
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
                .Include(x => x.Usuário)
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
                    separação.Embalagens.Any(x => x.StatusEmbalagem != StatusEmbalagem.Finalizado) &&
                    separação.Identificador != idSeparacao);

            if (separaçãoEmAberto != null)
            {
                var r3 = new RetornoDto("Usuário já está com outra separação em aberto", separaçãoEmAberto, "SEPARACAO_EM_ANDAMENTO");
                return Ok(r3);
            }

            var r5 = new RetornoDto("Separação pronta para ser embalada", workingSeparação);
            return Ok(r5);
        }

        //[HttpGet("assumidas"), Autorizar]
        //public async Task<IActionResult> GetSeparaçõesAssumidas()
        //{
        //    var provider = _scopeFactory.CreateScope().ServiceProvider;
        //    var context = provider.GetRequiredService<TrilhaDbContext>();

        //    object? userUuid = HttpContext.Items["user_info"];
        //    if (userUuid is not UserInfo userInfo)
        //    {
        //        var retorno1 = new RetornoDto("Uuid não foi determinada.");
        //        return Ok(new { retorno1.Mensagem, Codigo = "UUID_NAO_DETERMINADO" });
        //    }

        //    UserInfo? requestingUser = await context.Usuários.FirstOrDefaultAsync(x =>
        //        x.Uuid == userInfo.Uuid);

        //    if (requestingUser == null)
        //    {
        //        var retorno1 = new RetornoDto("Uuid não foi encontrada.");
        //        return Ok(new { retorno1.Mensagem, Codigo = "UUID_NAO_ENCONTRADO" });

        //    }

        //    var separações = await context.Separações
        //        .Where(x => x.Usuário != null && x.Usuário.Uuid == requestingUser.Uuid && x.Início != null)
        //        .Include(x => x.Embalagens)
        //        .ThenInclude(y => y.EmbalagemItems)
        //        .ToListAsync();

        //    if (separações.Count == 0)
        //    {
        //        var retorno1 = new RetornoDto("Nenhuma separação assumida foi encontrada.", null, "SEPARACAO_NAO_ENCONTRADA");
        //        return Ok(retorno1);
        //    }
        //    else
        //    {
        //        var retorno2 = new RetornoDto("Separações assumidas encontradas", new { usuario = requestingUser.DisplayName, embalagens = separações.SelectMany(x => x.Embalagens) });
        //        return Ok(retorno2);
        //    }
        //}
    }
}
