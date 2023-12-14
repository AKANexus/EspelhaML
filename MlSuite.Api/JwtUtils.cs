using System.IdentityModel.Tokens.Jwt;
using MlSuite.Domain.Enums;
using MlSuite.Domain;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.Api
{

	public class JwtUtils
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public JwtUtils(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		public string GenerateJwt(UserInfo userInfo, DateTime? expiryDate = null)
		{
			expiryDate ??= DateTime.UtcNow.AddHours(12);
			JwtSecurityTokenHandler tokenHandler = new();
			byte[] key = Encoding.UTF32.GetBytes(Secrets.JwtSecret);
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
				new Claim("uuid",
					userInfo.Uuid.ToString()),
				//new Claim("refreshToken",
				//	refreshToken),
				//new Claim("expires_on",
				//	((DateTime)expiryDate).Ticks.ToString())
				}),
				Expires = expiryDate,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature),
			};
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public async Task<(UserInfo? userInfo, DateTime? expiryDate, Exception? except)> ValidateJwtAsync(string token)
		{
			JwtSecurityTokenHandler tokenHandler = new();
			byte[] key = Encoding.UTF32.GetBytes(Secrets.JwtSecret);

			TokenValidationResult? validationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero,

			});

			if (validationResult.IsValid)
			{
				JwtSecurityToken jwToken = (JwtSecurityToken)validationResult.SecurityToken;
				Guid accountGuid = Guid.Parse(jwToken.Claims.First(x => x.Type == "uuid").Value);
				UserInfo? userInfo = await
					_scopeFactory.CreateScope()
						.ServiceProvider.GetRequiredService<TrilhaDbContext>()
						.Usuários.FirstOrDefaultAsync(x => x.Uuid == accountGuid);
				return (userInfo, jwToken.ValidTo, null);
			}

			else
			{
				return (null, null, validationResult.Exception);
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
			TrilhaDbContext dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<TrilhaDbContext>();
			bool isTokenUnique = await dbContext.RefreshTokens.AllAsync(x => x.Token == refreshToken.Token);
			if (!isTokenUnique)
			{
				return await GenerateRefreshToken(ipAddress);
			}
			return refreshToken;
		}
	}
}
