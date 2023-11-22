using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs
{
	public class PedidoDto
	{
		[JsonPropertyName("items")]
		public List<PedidoItemDto> PedidoItems { get; set; }
		[JsonPropertyName("num_pedido")]
		public ulong NúmPedido { get; set; }
	}

	public class PedidoItemDto
	{
		[JsonPropertyName("url_imagem")]
		public string UrlImagem { get; set; }
		[JsonPropertyName("descricao")]
		public string Descrição { get; set; }
		[JsonPropertyName("quantidade")]
		public int Quantidade { get; set; }
		[JsonPropertyName("cod_barras")]
		public string CódigoBarras { get; set; }
	}
}
