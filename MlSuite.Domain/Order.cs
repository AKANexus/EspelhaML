using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using MlSuite.Domain.Enums;

namespace MlSuite.Domain
{
    public class Order : EntityBase
    {
        public ulong Id { get; set; }
        public decimal? Frete { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItem> Itens { get; set; } = new();
        public Shipping? Shipping { get; set; }
        public List<Payment> Pagamentos { get; set; } = new();
        public ulong SellerId { get; set; }
        public Pack? Pack { get; set; }
    }

    public class OrderItem : EntityBase
    {
        public string Título { get; set; }
        public decimal PreçoUnitário { get; set; }
        public int QuantidadeVendida { get; set; }
        public Item? Item { get; set; }
        public string DescritorVariação { get; set; }
        public ItemVariação? ItemVariação { get; set; }
        public string Sku { get; set; } = "N/A";
    }

    public class Payment : EntityBase
    {
        public ulong Id { get; set; }
        public decimal TotalPago { get; set; }
        public decimal ValorTransação { get; set; }
        public decimal ValorRessarcido { get; set; }
        public int Parcelas { get; set; }
        public decimal ValorFrete { get; set; }

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
