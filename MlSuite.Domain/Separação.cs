using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
    public class Separação : EntityBase
    {
        public UserInfo? Gerador { get; set; }
        public UserInfo? Separador { get; set; }
        public UserInfo? Embrulhador { get; set; }
        public Guid PedidoId { get; set; }
        public List<Pedido> Pedidos { get; set; } = new();
        public DateTime Início { get; set; }
        public DateTime Fim { get; set; }
        public string? Etiqueta { get; set; }
        public StatusSeparação StatusSeparação { get; set; }
        public long NúmSeparação { get; set; }
    }

    public enum StatusSeparação
    {
        Aberta,
        Iniciada,
        Finalizado
    }
}
