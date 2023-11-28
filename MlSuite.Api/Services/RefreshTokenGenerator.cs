using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.Api.Services
{
	public class RefreshTokenGenerator
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public RefreshTokenGenerator(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		/// <summary>
		/// This will revoke all refresh tokens for the given user
		/// </summary>
		/// <param name="userGuid"></param>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public async Task<(string rt, DateTime expiryDateTime)> GenerateNewRefreshToken(Guid userGuid, string ipAddress)
		{
			var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<TrilhaDbContext>();
			RefreshToken newRefreshToken = new RefreshToken()
			{
				Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
				Expires = DateTime.UtcNow.AddHours(Env.RefreshTokenLifetimeHours),
				CreatorIp = ipAddress
			};
			while (await dbContext.RefreshTokens.AnyAsync(x=>x.Token == newRefreshToken.Token))
			{
				newRefreshToken.Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
			}
			foreach (RefreshToken refreshToken in 
			         await dbContext.RefreshTokens.Include(y=>y.UserInfo)
				         .Where(x=>x.UserInfo.Uuid == userGuid && x.IsActive).ToListAsync())
			{
				refreshToken.Revoked = DateTime.UtcNow;
				refreshToken.ReasonRevoked = "New token generated.";
				refreshToken.ReplacedByToken = newRefreshToken.Token;
				refreshToken.RevokerIp = ipAddress;
				dbContext.Update(refreshToken);
			}

			dbContext.Add(newRefreshToken);
			await dbContext.SaveChangesAsync();
			return (newRefreshToken.Token, newRefreshToken.Expires);
		}

		/// <summary>
		/// This will revoke all refresh tokens for the given user
		/// </summary>
		/// <param name="userGuid"></param>
		/// <param name="oldToken"></param>
		/// <param name="ipAddress"></param>
		/// <returns></returns>
		public async Task<(string token, DateTime expiration, UserInfo userInfo)?> RefreshTokenForANew(string oldToken, string ipAddress)
		{
			var dbContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<TrilhaDbContext>();

			RefreshToken? oldRefreshToken = await dbContext.RefreshTokens
				.Include(refreshToken => refreshToken.UserInfo)
				.FirstOrDefaultAsync(x => x.Token == oldToken);

			if (oldRefreshToken == null)
			{
				return null;
			}
			RefreshToken newRefreshToken = new RefreshToken()
			{
				Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
				Expires = DateTime.UtcNow.AddHours(Env.RefreshTokenLifetimeHours),
				CreatorIp = ipAddress,
				UserInfo = oldRefreshToken.UserInfo,
			};
			while (await dbContext.RefreshTokens.AnyAsync(x=>x.Token == newRefreshToken.Token))
			{
				newRefreshToken.Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
			}

			oldRefreshToken.Revoked = DateTime.UtcNow;
			oldRefreshToken.ReasonRevoked = "Token refreshed by client.";
			oldRefreshToken.ReplacedByToken = newRefreshToken.Token;
			oldRefreshToken.RevokerIp = ipAddress;

			dbContext.Update(oldRefreshToken);
			dbContext.Add(newRefreshToken);
			await dbContext.SaveChangesAsync();
			return (newRefreshToken.Token, newRefreshToken.Expires, newRefreshToken.UserInfo);
		}
	}
}
