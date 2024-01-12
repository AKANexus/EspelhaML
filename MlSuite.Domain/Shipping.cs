using MlSuite.Domain.Enums;

namespace MlSuite.Domain;

public class Shipping : EntityBase
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
    public string? Etiqueta { get; set; }
}
