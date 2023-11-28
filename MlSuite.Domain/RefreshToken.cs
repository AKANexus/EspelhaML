namespace MlSuite.Domain;

public class RefreshToken : EntityBase
{
    public UserInfo UserInfo { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string CreatorIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string? RevokerIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => Revoked == null && !IsExpired;
}