using System.Collections;
using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs
{
	public class RetornoDto
	{

		public RetornoDto(string mensagem, dynamic? dados = null, string código = "OK")
		{
			//Informação = informação;
			Mensagem = mensagem;
            Dados = dados;
            Código = código;

            //Sucesso = sucesso;
        }
		
		//[JsonPropertyName("informacao")]
		//public string Informação { get; set; }
		[JsonPropertyName("dados")]
		public dynamic? Dados { get; set; }
        [JsonPropertyName("codigo")]
        public string Código { get; set; }
        //[JsonPropertyName("sucesso")]
  //      public bool Sucesso { get; }

        [JsonPropertyName("mensagem")] 
		public string? Mensagem { get; set; }

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
