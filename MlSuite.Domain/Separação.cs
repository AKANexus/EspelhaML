using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
	public class Separação : EntityBase
	{
		public UserInfo Usuário { get; set; }
		public Guid PedidoId { get; set; }
		public Pedido Pedido { get; set; }
		public DateTime Início { get; set; }
		public DateTime Fim { get; set; }
		public string? Etiqueta { get; set; }
	}

	public class SeparaçãoItem : EntityBase
	{
		public int Separados { get; set; }
	}
}
