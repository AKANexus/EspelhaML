namespace MlSuite.Api.DTOs;

public class CriarSeparaçãoRootDto
{
    public List<CriarSeparaçãoItemDto> Pedidos { get; set; }
}

public class CriarSeparaçãoItemDto
{
    public string Tipo { get; set; }
    public ulong Id { get; set; }
}