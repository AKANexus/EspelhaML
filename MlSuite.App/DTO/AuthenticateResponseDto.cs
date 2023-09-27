using System.Text.Json.Serialization;

namespace MlSuite.App.DTO;

public class AuthenticateResponseDto
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
    public DateTime ExpiryOn { get; set; }
}