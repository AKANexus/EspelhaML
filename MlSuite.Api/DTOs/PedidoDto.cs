using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs
{
	public class PedidoDto
	{
		[JsonPropertyName("items")]
		public List<PedidoItemDto> PedidoItems { get; set; }
		[JsonPropertyName("num_pedido")]
		public ulong NúmPedido { get; set; }
		[JsonPropertyName("separado_por")] 
		public string? SeparadoPor { get; set; }
	}

	public class PedidoItemDto
	{
		[JsonPropertyName("url_imagem")]
		public string UrlImagem { get; set; }
		[JsonPropertyName("descricao")]
		public string Descrição { get; set; }
		[JsonPropertyName("quantidade")]
		public int Quantidade { get; set; }
		[JsonPropertyName("sku")]
		public string Sku { get; set; }
		[JsonPropertyName("separados")] 
		public int Separados { get; set; }
	}
}
