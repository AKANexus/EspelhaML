using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
    public class Pack : EntityBase
    {
        public List<Pedido> Pedidos { get; set; }
        public Envio Envio { get; set; }
    }
}
