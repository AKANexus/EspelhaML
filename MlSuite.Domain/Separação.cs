using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
	public class Separação : EntityBase
	{
		public UserInfo? Usuário { get; set; }
        public DateTime Início { get; set; }
		public DateTime Fim { get; set; }
		public long Identificador { get; set; }
        public List<Embalagem> Embalagens { get; set; }
        public ulong SellerId { get; set; }
	}


}
