using System.Text.Json.Serialization;

namespace EspelhaML.DTO
{
    public class NotificationDto
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        public string Resource { get; set; }
        public long UserId { get; set; }
        public string Topic { get; set; }
        public ulong ApplicationId { get; set; }
        public int Attempts { get; set; }
        public DateTime Sent { get; set; }
        public DateTime Received { get; set; }
    }
}
