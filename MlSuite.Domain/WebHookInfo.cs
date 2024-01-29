using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MlSuite.Domain.Enums;

namespace MlSuite.Domain
{
    public class WebHookInfo : EntityBase
    {
        public string CallbackUrl { get; set; }
        public WebHookTopic WebHookTopic { get; set; } = WebHookTopic.None;
    }
}
