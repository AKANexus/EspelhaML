using System.Security.Cryptography;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Api.Attributes;
using MlSuite.Api.DTOs;
using MlSuite.Api.Services;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.Api.Controllers
{
	[Route("auth"), Autorizar]
	public class AuthController : Controller
	{
		private readonly IServiceScopeFactory _scopeFactory;

		private string IpAddress()
		{
			if (Request.Headers.ContainsKey("X-Forwarded-For"))
				return (string?)Request?.Headers["X-Forwarded-For"] ?? "N/A";
			return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
		}

		public AuthController(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		[Anônimo, HttpPost("authenticate")]
		public async Task<IActionResult> Authenticate(AuthenticateRequestDto? dto)
		{
			if (dto == null)
			{
				return BadRequest(new RetornoDto("Falha ao autenticar", "Body inválido"));
			}

			IServiceScope scope = _scopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
			var userInfoTentativo =
				await dbContext.Usuários.FirstOrDefaultAsync(x => x.Username == dto.Login);
			if (userInfoTentativo == null)
			{
				return Unauthorized(new RetornoDto("Falha ao autenticar", "Usuário ou senha incorretos."));
			}

			if (!BCrypt.Net.BCrypt.Verify(dto.Password, userInfoTentativo.Password))
			{
				return Unauthorized(new RetornoDto("Falha ao autenticar", "Usuário ou senha incorretos."));
			}

			DateTime jwtExpiry = DateTime.UtcNow.AddHours(Env.ApiKeyLifeTimeHours);
			var jwtUtils = scope.ServiceProvider.GetRequiredService<JwtUtils>();
			var jwToken = jwtUtils.GenerateJwt(userInfoTentativo, jwtExpiry);
			var rtGenerator = scope.ServiceProvider.GetRequiredService<RefreshTokenGenerator>();
			var newRt = await rtGenerator.GenerateNewRefreshToken(userInfoTentativo.Uuid, IpAddress());
			Response.Cookies.Append("api_token", jwToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true
			});
			Response.Cookies.Append("refresh_token", newRt.rt, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				Expires = newRt.expiryDateTime
			});
			return Ok(new RetornoDto("Sucesso", "Login efetuado com sucesso!"));
		}

		[Anônimo, HttpGet("refreshKey")]
		public async Task<IActionResult> RefreshApiKey()
		{
			var rtToken = Request.Cookies["refresh_token"];
			if (rtToken == null)
			{
				return Unauthorized(new RetornoDto("Falha ao renovar api key", "Refresh token inválida."));
			}
			IServiceScope scope = _scopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
			var refreshTokenTentativo = await dbContext.RefreshTokens
				.Include(refreshToken => refreshToken.UserInfo).FirstOrDefaultAsync(x=>x.Token == rtToken);
			if (refreshTokenTentativo == null)
			{
				return Unauthorized(new RetornoDto("Falha ao renovar api key", "Refresh token inválida."));
			}

			DateTime jwtExpiry = DateTime.UtcNow.AddHours(Env.ApiKeyLifeTimeHours);
			var jwtUtils = scope.ServiceProvider.GetRequiredService<JwtUtils>();
			var jwToken = jwtUtils.GenerateJwt(refreshTokenTentativo.UserInfo, jwtExpiry);
			var rtGenerator = scope.ServiceProvider.GetRequiredService<RefreshTokenGenerator>();
			var newRt = await rtGenerator.GenerateNewRefreshToken(refreshTokenTentativo.UserInfo.Uuid, IpAddress());
			Response.Cookies.Append("api_token", jwToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true
			});
			Response.Cookies.Append("refresh_token", newRt.rt, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				Expires = newRt.expiryDateTime
			});
			return Ok(new RetornoDto("Sucesso", "Refresh efetuado com sucesso!"));
		}

		[Anônimo, HttpPost("preRegister")]
		public async Task<IActionResult> PreRegistro(PreRegisterAgentDto? dto)
		{
			if (dto == null)
			{
				return BadRequest(new RetornoDto("Falha ao pré-registrar", "Body inválido"));
			}

			ValidationResult result = await dto.Validator.ValidateAsync(dto);

			if (!result.IsValid)
			{
				return BadRequest(new RetornoDto("Falha ao pré-registrar", result.Errors));
			}

			IServiceScope scope = _scopeFactory.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
			var userTentativo = await dbContext.Usuários.FirstOrDefaultAsync(x => x.Email == dto.Email);
			if (userTentativo != null)
			{
				return BadRequest(new RetornoDto("Falha ao pré-registrar",
					"O email informado já está cadastrado no sistema."));
			}

			userTentativo = new UserInfo
			{
				Username = dto.Login!,
				Email = dto.Email!
			};

			var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
			while (await dbContext.Usuários.AnyAsync(x=>x.VerificationToken == token))
			{
				token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
			}
			userTentativo.VerificationToken = token;

			string message = $"<p>Você está recebendo essa mensagem porque seu e-mail foi utilizado para cadastro no MlSuite</p>" +
			                 $"" +
			                 $"<p>Para aceitar, e concluir o seu cadastro, <a href=\"http://www.example.com/auth/validateToken?token={userTentativo.VerificationToken}\">clique aqui</a>, ou cole o seguinte link no seu navegador:</p>" +
			                 $"" +
			                 $"<p>http://www.example.com/auth/validateToken?token={userTentativo.VerificationToken}</p>";
			EmailService emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
			
			try
			{
				await emailService.SendAsync(dto.Email!, "Cadastro em Trilhadesk", message, "naoresponda@trilhadesk.com");
				dbContext.Update(userTentativo);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return StatusCode(500, new RetornoDto("Falha ao pré-registrar usuário", dados: e));
			}

			return Ok(new RetornoDto("Sucesso!", "Usuário pré-registrado com sucesso!"));
		}

        [Anônimo, HttpPost("validateToken")]
        public async Task<IActionResult> ValidateToken(string? token, string? redirectUrl)
        {
            if (token == null)
            {
                return BadRequest(new RetornoDto("Falha ao verificar a token", "Token inválida"));
            }

            IServiceScope scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var userTentativo = await dbContext.Usuários.FirstOrDefaultAsync(x => x.VerificationToken == token);

            if (userTentativo == null)
            {
                return BadRequest(new RetornoDto("Falha ao verificar a token", "Token inválida"));
            }

            return Redirect($"{redirectUrl}?user={userTentativo.Uuid}");
        }

        [Anônimo, HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterAgentDto? dto)
        {
            if (dto == null)
            {
                return BadRequest(new RetornoDto("Falha ao pré-registrar", "Body inválido"));
            }

            if (dto.Uuid == null || string.IsNullOrWhiteSpace(dto.DisplayName) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new RetornoDto("Falha ao pré-registrar", "Body incompleto"));
            }

            IServiceScope scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var userTentativo = await dbContext.Usuários.FirstOrDefaultAsync(x => x.Uuid == dto.Uuid);

            if (userTentativo == null)
            {
                return BadRequest(new RetornoDto("Falha ao registrar usuário", "Guid inválido"));
            }

			userTentativo.DisplayName = dto.DisplayName;
            userTentativo.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            userTentativo.VerificationToken = null;
            dbContext.Update(userTentativo);
			await dbContext.SaveChangesAsync();

            return Ok(new RetornoDto("Sucesso!", "Usuário registrado com sucesso!"));
        }
	}
}
