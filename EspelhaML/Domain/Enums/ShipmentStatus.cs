namespace EspelhaML.Domain.Enums
{
    public enum ShipmentStatus
    {
        Pendente,
        FretePago,
        Autorizado,
        Enviado,
        Entregue,
        NãoEntregue,
        Cancelado
    }

    public enum ShipmentSubStatus
    {
        Impresso,
        Coletado,
        AutorizadoPelaTransportadora,
        NoHub
    }
}
