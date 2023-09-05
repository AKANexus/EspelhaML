using EspelhaML.Domain.Enums;

namespace EspelhaML.Domain
{
    public class Pedido : EntityBase
    {
        public ulong Id { get; set; }
        public decimal? Frete { get; set; }
        public OrderStatus Status { get; set; }
        public List<PedidoItem> Itens { get; set; } = new();
        public PedidoEnvio? Envio { get; set; }
        public List<PedidoPagamento> Pagamentos { get; set; } = new();
    }

    public class PedidoItem
    {
        public string Título { get; set; }
        public decimal PreçoUnitário { get; set; }
        public int QuantidadeVendida { get; set; }
        public Item? Item { get; set; }
        public string DescritorVariação { get; set; }
        public ItemVariação? ItemVariação { get; set; }
    }

    public class PedidoPagamento
    {
        public ulong Id { get; set; }
        public decimal TotalPago { get; set; }
        public decimal ValorTransação { get; set; }
        public decimal ValorRessarcido { get; set; }
        public int Parcelas { get; set; }
        public decimal ValorFrete { get; set; }

    }

    public class PedidoEnvio
    {
        public ulong Id { get; set; }
        public ShipmentStatus Status { get; set; }
        public ShipmentSubStatus? SubStatus { get; set; }
        public string SubStatusDescrição { get; set; }
        public DateTime CriaçãoDoPedido { get; set; }
        public decimal ValorDeclarado { get; set; }
        public decimal Largura { get; set; }
        public decimal Altura { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Peso { get; set; }
        public string CódRastreamento { get; set; }
        public PedidoDestinatário Destinatário { get; set; }
    }

    public class PedidoDestinatário
    {
        public ulong Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Logradouro { get; set; }
        public string Número { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Bairro { get; set; }
        public string Distrito { get; set; }
        public bool ÉResidencial { get; set; }
    }
}
