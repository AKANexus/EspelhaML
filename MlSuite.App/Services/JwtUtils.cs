using Microsoft.Extensions.Options;
using MlSuite.App.DTO.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MlSuite.Domain;
using MlSuite.Domain.Enums;

namespace MlSuite.App.Services;

public class JwtUtils
{
    private readonly AppSettings _settings;
    private readonly AccountBaseDataService _accountBaseDataService;

    public JwtUtils(IOptions<AppSettings> settings, IServiceProvider provider)
    {
        _settings = settings.Value;
        _accountBaseDataService = provider.GetRequiredService<AccountBaseDataService>();
    }

    public string GenerateJwt(AccountBase account, DateTime? expiryDate = null)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(_settings.JwtSecret);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id",
                    account.Uuid.ToString()),
                new Claim("role",
                    account.Role.ToString()),
                //new Claim("tenant",
                //    account.Tenant.Uuid.ToString())
            }),
            Expires = expiryDate ?? DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<(Guid uuid, Role role, Guid tenant)?> ValidateJwt(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(_settings.JwtSecret);
        try
        {
            TokenValidationResult? validationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });

            if (validationResult.IsValid)
            {
                JwtSecurityToken jwToken = (JwtSecurityToken)validationResult.SecurityToken;
                Guid accountGuid = Guid.Parse(jwToken.Claims.First(x => x.Type == "id").Value);
                Role accountRole = Enum.Parse<Role>(jwToken.Claims.First(x => x.Type == "role").Value);
                //Guid tenantGuid = Guid.Parse(jwToken.Claims.First(x => x.Type == "tenant").Value);

                return (accountGuid, accountRole, Guid.Empty);
            }

            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
    {
        RefreshToken refreshToken = new RefreshToken
        {
            Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            CreatorIp = ipAddress
        };

        bool isTokenUnique = await _accountBaseDataService.IsTokenUnique(refreshToken.Token);
        if (!isTokenUnique)
        {
            return await GenerateRefreshToken(ipAddress);
        }
        return refreshToken;
    }
}