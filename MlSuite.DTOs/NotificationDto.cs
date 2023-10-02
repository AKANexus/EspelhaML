using System.Text.Json.Serialization;

namespace MlSuite.DTOs
{
    public class NotificationDto
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        public string Resource { get; set; }
        [JsonPropertyName("user_id")]
        public ulong UserId { get; set; }
        public string Topic { get; set; }
        [JsonPropertyName("application_id")]
        public ulong ApplicationId { get; set; }
        public int Attempts { get; set; }
        public DateTime Sent { get; set; }
        public DateTime Received { get; set; }
    }
}
