using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MlSuite.Domain
{
    public class UserInfo : EntityBase
    {
        public string Username { get; set; }
        public string? Password { get; set; }
        public string? DisplayName { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string? VerificationToken { get; set; }
    }
}
