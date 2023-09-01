using System.Text.Json.Serialization;

namespace MLPerguntas.Models.Dto.MercadoLivre
{
    public class AccessTokenDto
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("error_description")]
        public string? ErrorDescription { get; set; }
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        [JsonPropertyName("status")]
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
        [JsonConstructor]
        public AccessTokenDto()
        {

        }
    }
}
