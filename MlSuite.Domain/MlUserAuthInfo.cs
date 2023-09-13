using System.ComponentModel.DataAnnotations.Schema;

namespace MlSuite.Domain
{
    public class MlUserAuthInfo : EntityBase
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public long UserId { get; set; }
        public string RefreshToken { get; set; }
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow > ExpiresOn;

        public string AccountNickname { get; set; }
        public string AccountRegistry { get; set; }

        public MlUserAuthInfo(string accessToken, DateTime expiresOn, long userId, string refreshToken, string accountNickname, string accountRegistry)
        {
            AccessToken = accessToken;
            ExpiresOn = expiresOn;
            UserId = userId;
            RefreshToken = refreshToken;
            AccountNickname = accountNickname;
            AccountRegistry = accountRegistry;
        }
    }
}
