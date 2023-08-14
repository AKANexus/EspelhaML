namespace EspelhaML.Domain
{
    public class MlUserAuthInfo : EntityBase
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }

        public MlUserAuthInfo(string accessToken, DateTime expiresOn, int userId, string refreshToken)
        {
            AccessToken = accessToken;
            ExpiresOn = expiresOn;
            UserId = userId;
            RefreshToken = refreshToken;
        }
    }
}
