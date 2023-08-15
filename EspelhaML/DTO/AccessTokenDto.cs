namespace EspelhaML.DTO
{
    public class AccessTokenDto
    {
        public string? AccessToken { get; set; }
        public string? TokenType { get; set; }
        public int? ExpiresIn { get; set; }
        public string? Scope { get; set; }
        public long? UserId { get; set; }
        public string? RefreshToken { get; set; }
        public string? ErrorDescription { get; set; }
        public string? Error { get; set; }
        public int? Status { get; set; }
        

        public AccessTokenDto(string accessToken, string tokenType, int expiresIn, string scope, long userId,
            string refreshToken)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
            Scope = scope;
            UserId = userId;
            RefreshToken = refreshToken;
        }
    }
}
