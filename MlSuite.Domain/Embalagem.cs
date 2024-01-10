using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
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
        Aberto,
        Iniciado,
        Impresso
    }

    public class Embalagem : EntityBase
    {
        public string? Etiqueta { get; set; }
        public ulong ShippingId { get; set; }
        public ulong ReferenciaId { get; set; }
        public TipoVendaMl TipoVendaMl { get; set; }
        public List<EmbalagemItem> EmbalagemItems { get; set; }
        public StatusEmbalagem StatusEmbalagem { get; set; }
    }

    public class EmbalagemItem : EntityBase
    {
        public string SKU { get; set; }
        public string ImageUrl { get; set; }
        public string Descrição { get; set; }
        public int QuantidadeAEscanear { get; set; }
        public int QuantidadeEscaneada { get; set; }
    }
}
