using System.Text.Json.Serialization;
using MlSuite.Domain.Enums;

namespace MlSuite.Api.DTOs
{
	public class PedidoDto
    {
        [JsonPropertyName("items")] public List<PedidoItemDto> PedidoItems { get; set; } = new();
		[JsonPropertyName("num_pedido")]
		public ulong NúmPedido { get; set; }
		[JsonPropertyName("separado_por")] 
		public string? SeparadoPor { get; set; }

		[JsonPropertyName("tipo_envio")] 
        public ShipmentType TipoEnvio { get; set; }
		[JsonPropertyName("conta_ml")]
		public string MlUsername { get; set; }
		[JsonPropertyName("pack_id")]
		public ulong? PackId { get; set; }
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
