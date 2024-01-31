using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs;

public class CriarSeparaçãoRootDto
{
    [JsonPropertyName("pedidos")]
    public List<CriarSeparaçãoItemDto> Pedidos { get; set; }
}

public class CriarSeparaçãoItemDto
{
    [JsonPropertyName("tipo")]
    public string Tipo { get; set; }
    [JsonPropertyName("id")]
    public ulong Id { get; set; }
}