namespace EspelhaML.Domain
{
    public class MlUserAuthInfo : EntityBase
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public long UserId { get; set; }
        public string RefreshToken { get; set; }

        public MlUserAuthInfo(string accessToken, DateTime expiresOn, long userId, string refreshToken)
        {
            AccessToken = accessToken;
            ExpiresOn = expiresOn;
            UserId = userId;
            RefreshToken = refreshToken;
        }
    }
}
