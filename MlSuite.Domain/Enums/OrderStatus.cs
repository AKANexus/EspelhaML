namespace MlSuite.Domain.Enums
{
    public enum OrderStatus
    {
        Desconhecido = -1,
        Confirmado = 1,
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
