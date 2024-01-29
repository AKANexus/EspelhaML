using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain.Enums
{
    [Flags]
    public enum WebHookTopic
    {
        None = 0,
        SeparaçãoCriada = 1 << 0,
        SeparaçãoAssumida = 1 << 1,
        SeparaçãoConcluída = 1 << 2,
    }
}
