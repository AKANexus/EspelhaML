using MlSuite.Domain.Enums.JsonConverters;
using MlSuite.Domain.Enums;
using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs
{
    public class WebHookCallDto
    {
        [JsonPropertyName("timestamp_evento")]
        public DateTime TimestampEvento { get; set; }
        [JsonConverter(typeof(EnumStringConverter<WebHookTopic>)), JsonPropertyName("topico")]
        public WebHookTopic Tópico { get; set; }
        [JsonPropertyName("uuid_registro")]
        public Guid UuidRegistro { get; set; }

        public WebHookCallDto(DateTime timestampEvento, WebHookTopic tópico, Guid uuidRegistro)
        {
            TimestampEvento = timestampEvento;
            Tópico = tópico;
            UuidRegistro = uuidRegistro;
        }
    }
}
