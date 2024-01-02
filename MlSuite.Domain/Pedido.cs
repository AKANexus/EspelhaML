using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using MlSuite.Domain.Enums;

namespace MlSuite.Domain
{
    public class Pedido : EntityBase
    {
        public ulong Id { get; set; }
        public decimal? Frete { get; set; }
        public OrderStatus Status { get; set; }
        public List<PedidoItem> Itens { get; set; } = new();
        public Envio Envio { get; set; } = new();
        public List<PedidoPagamento> Pagamentos { get; set; } = new();
        public ulong SellerId { get; set; }
        public Pack? Pack { get; set; }
        public Separação? Separação { get; set; } = new();

        public bool ProntoParaImpressão()
        {
            return false;
        }

        public bool ProntoParaSeparação()
        {
            return Envio is { Status: ShipmentStatus.ProntoParaEnvio, SubStatus: ShipmentSubStatus.ProntoParaColeta or ShipmentSubStatus.Impresso } &&
                   Envio.SubStatusDescrição != "invoice_pending" &&
                   Envio.SubStatusDescrição != "picked_up" &&
                   Envio.TipoEnvio != ShipmentType.Fulfillment;
        }
    }

    public class PedidoItem : EntityBase
    {
        public string Título { get; set; }
        public decimal PreçoUnitário { get; set; }
        public int QuantidadeVendida { get; set; }
        public Item? Item { get; set; }
        public string DescritorVariação { get; set; }
        public ItemVariação? ItemVariação { get; set; }
        public string Sku { get; set; } = "N/A";
        public EmbalagemItem? EmbalagemItem { get; set; }
        public Pedido Pedido { get; set; }
    }

    public class PedidoPagamento : EntityBase
    {
        public ulong Id { get; set; }
        public decimal TotalPago { get; set; }
        public decimal ValorTransação { get; set; }
        public decimal ValorRessarcido { get; set; }
        public int Parcelas { get; set; }
        public decimal ValorFrete { get; set; }

    }

    public class Envio : EntityBase
    {
        public ulong Id { get; set; }
        public ShipmentStatus Status { get; set; }
        public ShipmentSubStatus? SubStatus { get; set; }
        public string? SubStatusDescrição { get; set; }
        public DateTime? CriaçãoDoPedido { get; set; }
        public decimal? ValorDeclarado { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Peso { get; set; }
        public string? CódRastreamento { get; set; }
        public ShipmentType TipoEnvio { get; set; }
        public PedidoDestinatário? Destinatário { get; set; }
        public List<Pedido> Pedidos { get; set; } = new();
        public Embalagem Embalagem { get; set; }
    }

    public class PedidoDestinatário : EntityBase
    {
        public ulong? Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Logradouro { get; set; }
        public string Número { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Bairro { get; set; }
        public string? Distrito { get; set; }
        public bool ÉResidencial { get; set; }
    }
}
