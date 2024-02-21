using MlSuite.Domain.Enums.JsonConverters;
using MlSuite.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
    public enum TipoVendaMl
    {
        Pack,
        Order
    }

    public enum StatusEmbalagem
    {
        Designado,
        Iniciado,
        Finalizado
    }

    public class Embalagem : EntityBase
    {
        public string? Etiqueta { get; set; }
        [ForeignKey(nameof(Shipping))]
        public Guid ShippingUuid { get; set; }
        [JsonIgnore]
        public Shipping Shipping { get; set; }
        public ulong ReferenciaId { get; set; }
        public TipoVendaMl TipoVendaMl { get; set; }
        public List<EmbalagemItem> EmbalagemItems { get; set; }
        public StatusEmbalagem StatusEmbalagem { get; set; }
        public DateTime? TimestampImpressao { get; set; }
    }

    public class EmbalagemItem : EntityBase
    {
        public string SKU { get; set; }
        [JsonPropertyName("url_imagem")]
        public string ImageUrl { get; set; }
        public string Descrição { get; set; }
        [JsonPropertyName("quantidade")]
        public int QuantidadeAEscanear { get; set; }
        [JsonPropertyName("separados")]
        public int QuantidadeEscaneada { get; set; }
    }
}
