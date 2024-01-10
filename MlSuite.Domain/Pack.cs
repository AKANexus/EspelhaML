using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
    public class Pack : EntityBase
    {
        public List<Order> Pedidos { get; set; }
        public Shipping Shipping { get; set; }
        public ulong Id { get; set; }
    }
}
