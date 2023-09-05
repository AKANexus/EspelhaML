namespace EspelhaML.Domain.Enums
{
    public enum OrderStatus
    {
        Confirmado,
        PagamentoNecessário,
        PagamentoEmProcesso,
        PagoParcial,
        Pago,
        RessarcidoParcial,
        NãoRessarcido,
        Cancelada,
        Ilegal
    }
}
