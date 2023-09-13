namespace MlSuite.Domain.Enums
{
    public enum ShipmentStatus
    {
        Desconhecido = -1,
        Pendente = 1,
        FretePago,
        Autorizado,
        Enviado,
        Entregue,
        NãoEntregue,
        Cancelado
    }

    public enum ShipmentSubStatus
    {
        Desconhecido = -1,
        Impresso = 1,
        Coletado,
        AutorizadoPelaTransportadora,
        NoHub,
        EmRotaEntrega,
        RetornandoAoVendedor,
        FulfilledFeedback
    }
}
