using Microsoft.IdentityModel.Tokens;
using MlSuite.Api.Services;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.Api.Middlewares
{
	public class JwtMware
	{
		private readonly RequestDelegate _next;

		public JwtMware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext, JwtUtils jwtUtils, RefreshTokenGenerator rTokenGenerator)
		{
			string? token = httpContext.Request.Cookies["api_token"];
			if (token == null)
			{
				httpContext.Items["success"] = false;
				httpContext.Items["error"] = new SecurityTokenException("Token inválida");
				await _next(httpContext);
				return;
			}

			(UserInfo? userInfo, DateTime? expiryDate, Exception? except) jwtDecodedResult =
				await jwtUtils.ValidateJwt(token);

			if (jwtDecodedResult.except != null)
			{
				if (jwtDecodedResult.except is SecurityTokenExpiredException)
				{
					var rtCookie = httpContext.Request.Cookies["refresh_token"];
					if (rtCookie == null)
					{
						httpContext.Items["success"] = false;
						httpContext.Items["error"] = new Exception("Refresh Token vencida");
						await _next(httpContext);
						return;
					}
					(string token, DateTime expiration, UserInfo userInfo)? newToken = await rTokenGenerator.RefreshTokenForANew(rtCookie, IpAddress());
					if (newToken == null)
					{
						httpContext.Items["success"] = false;
						httpContext.Items["error"] = new Exception("Falha ao renovar a RT");
						await _next(httpContext);
						return;
					}
					httpContext.Response.Cookies.Append("refresh_token", newToken.Value.token, new CookieOptions
					{
						Expires = newToken.Value.expiration,
						HttpOnly = true
					});

					var newJwt = jwtUtils.GenerateJwt(newToken.Value.userInfo, DateTime.UtcNow.AddHours(1));
					httpContext.Response.Cookies.Append("api_token", newToken.Value.token, new CookieOptions
					{
						HttpOnly = true
					});
					httpContext.Items["success"] = true;
					httpContext.Items["userInfo"] = newToken.Value.userInfo;
					await _next(httpContext);
					return;
				}
			}
			else
			{
				httpContext.Items["success"] = true;
				httpContext.Items["userInfo"] = jwtDecodedResult.userInfo;
				await _next(httpContext);
				return;
			}

			string IpAddress()
			{
				if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
					return (string?)httpContext.Request?.Headers["X-Forwarded-For"] ?? "N/A";
				return httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
			}

		}
	}
}
