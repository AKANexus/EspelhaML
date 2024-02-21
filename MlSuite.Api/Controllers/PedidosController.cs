using System.Globalization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Api.Attributes;
using MlSuite.Api.DTOs;
using MlSuite.Domain;
using MlSuite.Domain.Enums;
using MlSuite.Domain.Enums.JsonConverters;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.Api.Controllers
{

    public class BuscaPedidosFiltroDto
    {
        public long? numPedido { get; set; }
        public string? dataInicio { get; set; }
        public string? dataFim { get; set; }
        public long? sellerId { get; set; }
        [JsonConverter(typeof(EnumStringConverter<PedidoStatus>))]
        public PedidoStatus? status { get; set; }
        [JsonConverter(typeof(EnumStringConverter<ShipmentType>))]

        public ShipmentType? tipoEnvio { get; set; }

        public int? take { get; set; } = 15;
        public int? skip { get; set; } = 0;
    }

    public class PedidoRetornoDto
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("tipo_venda")]
        public string TipoVenda { get; set; }
        [JsonPropertyName("seller_id")]
        public ulong SellerId { get; set; }
        [JsonPropertyName("seller_nick")]
        public string SellerNick { get; set; }
        [JsonPropertyName("itens")]
        public List<PedidoItemRetornoDto> Itens { get; set; }
    }

    public class PedidoItemRetornoDto
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }
        [JsonPropertyName("url_imagem")]
        public string UrlImagem { get; set; }
        [JsonPropertyName("descrição")]
        public string Descrição { get; set; }
        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }
    }


    [Route("pedidos"), EnableCors, Autorizar]
    public class PedidosController : Controller
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PedidosController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        
        [HttpGet("buscar"), AllowAnonymous]
        public async Task<IActionResult> BuscarPedidos(BuscaPedidosFiltroDto dto)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            IQueryable<Order> initialQuery = context.Orders
                //.AsNoTracking()
                .Include(x => x.Itens)
                .ThenInclude(orderItem => orderItem.Item)
                .Include(x => x.Shipping)
                .Include(x => x.Pack)
                .ThenInclude(y => y.Pedidos)
                .ThenInclude(z => z.Itens)
                .ThenInclude(w => w.Item)

                ;

            if (dto.tipoEnvio != null)
            {
                initialQuery = initialQuery.Where(x => x.Shipping.TipoEnvio == dto.tipoEnvio);
            }

            switch (dto.status)
            {
                case PedidoStatus.PendenciaML:
                    initialQuery = initialQuery.Where(x =>
                        x.Shipping == null ||
                        (x.Shipping.Status != ShipmentStatus.ProntoParaEnvio || x.Shipping.SubStatus != ShipmentSubStatus.ProntoParaImpressão) &&
                        x.Shipping.Etiqueta == null
                        );
                    break;
                case PedidoStatus.ProntoParaSeparacao:
                    initialQuery = initialQuery.Where(x => x.Shipping != null &&
                                                           x.Shipping.Status == ShipmentStatus.ProntoParaEnvio &&
                                                           x.Shipping.SubStatus ==
                                                           ShipmentSubStatus.ProntoParaImpressão &&
                                                           x.Shipping.Embalagem == null);

                    break;
                case PedidoStatus.EmLista:
                    initialQuery = initialQuery.Where(x => x.Shipping != null &&
                                                           x.Shipping.Status == ShipmentStatus.ProntoParaEnvio &&
                                                           x.Shipping.SubStatus ==
                                                           ShipmentSubStatus.ProntoParaImpressão &&
                                                           x.Shipping.Embalagem != null);
                    break;
                case PedidoStatus.Processado:
                    initialQuery = initialQuery.Where(x =>
                        x.Shipping == null ||
                        (x.Shipping.Status != ShipmentStatus.ProntoParaEnvio ||
                         x.Shipping.SubStatus != ShipmentSubStatus.ProntoParaImpressão) &&
                        x.Shipping.Etiqueta != null
                        );
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (dto.sellerId != null)
            {
                initialQuery = initialQuery.Where(x => x.SellerId == (ulong)dto.sellerId);
            }

            if (dto.numPedido != null)
            {
                initialQuery = initialQuery.Where(x => x.Id == (ulong)dto.numPedido);
            }

            if (dto.dataInicio != null && DateOnly.TryParseExact(dto.dataInicio, "YYYY-MM-dd", out DateOnly dataInicio))
            {
                initialQuery = initialQuery.Where(x => x.CreatedAt >= dataInicio.ToDateTime(TimeOnly.MinValue));
            }

            if (dto.dataFim != null && DateOnly.TryParseExact(dto.dataFim, "YYYY-MM-dd", out DateOnly dataFim))
            {
                initialQuery = initialQuery.Where(x => x.CreatedAt <= dataFim.ToDateTime(TimeOnly.MaxValue));
            }

            int numRegistros = await initialQuery
                .GroupBy(a => a.Shipping.Id)
                .Select(group => group.First()).CountAsync();
            int numPags = numRegistros / (dto.take ?? 15);

            List<PedidoRetornoDto> Pedidos = new(100);
            Dictionary<ulong, MlUserAuthInfo> Sellers = await context.MlUserAuthInfos.ToDictionaryAsync(x => x.UserId);
            foreach (Order order in await initialQuery.GroupBy(a => a.Shipping.Id)
                         .Select(group => group.First()).Skip(dto.skip ?? 10).Take(dto.take ?? 15).ToListAsync())
            {
                if (Pedidos.Count == 10) break;
                if (order.Pack != null)
                {
                    Pedidos.Add(new PedidoRetornoDto()
                    {
                        Id = order.Pack.Id,
                        SellerNick = Sellers[order.SellerId].AccountNickname,
                        SellerId = order.SellerId,
                        TipoVenda = TipoVendaMl.Pack.ToString(),
                        Itens = order.Pack.Pedidos.SelectMany(x => x.Itens.Select(y => new PedidoItemRetornoDto()
                        {
                            Descrição = y.Título,
                            Sku = y.Sku,
                            Quantidade = y.QuantidadeVendida,
                            UrlImagem = y.Item.PrimeiraFoto
                        })).ToList(),
                    });
                }
                else
                {
                    Pedidos.Add(new PedidoRetornoDto()
                    {
                        Id = order.Id,
                        SellerNick = Sellers[order.SellerId].AccountNickname,
                        SellerId = order.SellerId,
                        TipoVenda = TipoVendaMl.Order.ToString(),
                        Itens = order.Itens.Select(y => new PedidoItemRetornoDto()
                        {
                            Descrição = y.Título,
                            Sku = y.Sku,
                            Quantidade = y.QuantidadeVendida,
                            UrlImagem = y.Item.PrimeiraFoto
                        }).ToList(),
                    });
                }
            }

            if (Pedidos.Count > 0)
            {
                var r0 = new RetornoDto("Pedidos encontrados", new { Pedidos, Paginas = numPags, DEBUG = Pedidos.Count });
                return Ok(r0);
            }

            var r1 = new RetornoDto("Nenhum pedido encontrado com o filtro aplicado");
            return Ok(r1);
        }
        
    }
}
