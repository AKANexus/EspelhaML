using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
    public class Embalagem : EntityBase
    {
        //public List<EmbalagemItem> EmbalagemItems { get; set; }
        public bool Aberto { get; set; }
    }

    public class EmbalagemItem : EntityBase
    {
        public int Separados { get; set; }
        public PedidoItem PedidoItem { get; set; }
    }

}
