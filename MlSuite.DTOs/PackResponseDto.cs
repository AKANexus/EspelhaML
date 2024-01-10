using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MlSuite.MlDTOs
{
    public class PackResponseDto : ErrorDto
    {
        public long Id { get; set; }

        public string Status { get; set; }

        [JsonPropertyName("status_detail")]
        public string StatusDetail { get; set; }

        [JsonPropertyName("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }

        [JsonPropertyName("family_pack_id")]
        public object FamilyPackId { get; set; } // Pode ajustar o tipo conforme necessário

        public Buyer Buyer { get; set; }

        public ShipmentDto Shipment { get; set; }

        public List<OrderRootDto> Orders { get; set; }
    }
}
