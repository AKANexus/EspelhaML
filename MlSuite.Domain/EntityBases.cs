using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using MlSuite.Domain.Enums;

namespace MlSuite.Domain
{
    public abstract class EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uuid { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public abstract class AccountBase : EntityBase
    {
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public string? VerificationToken { get; set; }
        [JsonIgnore]
        public DateTime? VerifiedAt { get; set; }
        [JsonIgnore]
        public string? PasswordResetToken { get; set; }
        [JsonIgnore]
        public DateTime? ResetTokenExpires { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; } = new();
        public string Name { get; set; }
        public string Email { get; set; }

        protected AccountBase()
        {

        }
        protected AccountBase(string username, string password, Role role, string name, string email)
        {
            Username = username;
            Password = password;
            Role = role;
            Name = name;
            Email = email;
        }


    }
}
