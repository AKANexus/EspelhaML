using System.Collections;
using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs
{
	public class RetornoDto
	{

		public RetornoDto(string status, string erro)
		{
			Status = status;
			Erro = erro;
		}

		public RetornoDto(string status, dynamic dados)
		{
			Status = status;
			Dados = dados;
		}

		[JsonPropertyName("status")]
		public string Status { get; set; }
		[JsonPropertyName("dados")]
		public dynamic? Dados { get; set; }
		[JsonPropertyName("erro")] 
		public string? Erro { get; set; }

		[JsonPropertyName("registros")]
		public int? Registros
		{
			get
			{
				if (Dados is IEnumerable)
					return Dados.Count;
				if (Dados is not null)
					return 1;
				else
					return 0;
			}
		}
	}
}
