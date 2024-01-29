using System.Security.Cryptography;
using System.Text.Json.Serialization;
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
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateRequestDto? dto)
        {
            if (dto == null)
            {
                var retorno1 = new RetornoDto("Body inválido");
                return BadRequest(new {retorno1.Mensagem, retorno1.Registros, Codigo = "BODY_INVALIDO"});
            }

            IServiceScope scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var userInfoTentativo =
                await dbContext.Usuários.FirstOrDefaultAsync(x => EF.Functions.ILike(x.Username, dto.Login));
            if (userInfoTentativo == null)
            {
                var retorno1 = new RetornoDto("Usuário ou senha incorretos.");
                return Unauthorized(new {retorno1.Mensagem, retorno1.Registros, Codigo = "WRONG_CREDENTIALS"});
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, userInfoTentativo.Password))
            {
                var retorno1 = new RetornoDto("Usuário ou senha incorretos.");
                return Unauthorized(new {retorno1.Mensagem, retorno1.Registros, Codigo = "WRONG_CREDENTIALS"});
            }

            DateTime jwtExpiry = DateTime.UtcNow.AddHours(Env.ApiKeyLifeTimeHours);
            var jwtUtils = scope.ServiceProvider.GetRequiredService<JwtUtils>();
            var jwToken = jwtUtils.GenerateJwt(userInfoTentativo, jwtExpiry);
            var rtGenerator = scope.ServiceProvider.GetRequiredService<RefreshTokenGenerator>();
            var newRt = await rtGenerator.GenerateNewRefreshToken(userInfoTentativo.Uuid, IpAddress());
            var retorno2 = new RetornoDto("Login efetuado com sucesso!",
                new
                {
                    api_token = jwToken, refresh_token = newRt.rt, expires_on = jwtExpiry,
                    display_name = userInfoTentativo.DisplayName
                });
            return Ok(new {retorno2.Registros, retorno2.Mensagem, Codigo = "OK", auth_info = retorno2.Dados});
        }



        [Anônimo, HttpPost("refreshKey")]
        public async Task<IActionResult> RefreshApiKey([FromBody]RefreshApiKeyDto? dto)
        {
            if (dto == null)
            {
                var retorno1 = new RetornoDto("Refresh token inválida - 77.");
                return BadRequest(new {retorno1.Mensagem, retorno1.Registros, Codigo = "RT_INVALIDA"});
            }

            IServiceScope scope = _scopeFactory.CreateScope();
            var jwtUtils = scope.ServiceProvider.GetRequiredService<JwtUtils>();
            var rtToken = dto.RefreshToken;
            if (rtToken == null || dto.ApiKey == null)
            {
                var retorno1 = new RetornoDto("Refresh token inválida - 86.");
                return BadRequest(new {retorno1.Mensagem, retorno1.Registros, Codigo = "RT_INVALIDA"});
            }
            var oldJwt = await jwtUtils.ValidateJwtAsync(dto.ApiKey);
            if (oldJwt.except != null || oldJwt.userInfo == null)
            {
                var retorno1 = new RetornoDto($"Api token token inválida - 92\n{oldJwt.except}.");
                return Unauthorized(new {retorno1.Mensagem, retorno1.Registros, Codigo = "APIKEY_INVALIDA"});
            }
            var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var userTentativo = await dbContext.Usuários.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Uuid == oldJwt.userInfo.Uuid);
            if (userTentativo == null)
            {
                var retorno1 = new RetornoDto("Api token token inválida - 100.");
                return Unauthorized(new {retorno1.Mensagem, retorno1.Registros, Codigo = "APIKEY_INVALIDA"});
            }
            var refreshTokenTentativo = await dbContext.RefreshTokens
                .Include(refreshToken => refreshToken.UserInfo).FirstOrDefaultAsync(x => x.Token == rtToken);
            if (refreshTokenTentativo == null)
            {
                var retorno1 = new RetornoDto("Refresh token inválida - 107.");
                return Unauthorized(new {retorno1.Mensagem, retorno1.Registros, Codigo = "RT_INVALIDA"});
            }

            DateTime jwtExpiry = DateTime.UtcNow.AddHours(Env.ApiKeyLifeTimeHours);
            var jwToken = jwtUtils.GenerateJwt(refreshTokenTentativo.UserInfo, jwtExpiry);
            var rtGenerator = scope.ServiceProvider.GetRequiredService<RefreshTokenGenerator>();
            var newRt = await rtGenerator.GenerateNewRefreshToken(refreshTokenTentativo.UserInfo.Uuid, IpAddress());
            var retorno2 = new RetornoDto("Login efetuado com sucesso!",
                new
                {
                    api_token = jwToken, refresh_token = newRt.rt, expires_on = jwtExpiry,
                    display_name = refreshTokenTentativo.UserInfo.DisplayName
                });
            return Ok(new {retorno2.Registros, retorno2.Mensagem, Codigo = "OK", auth_info = retorno2.Dados});        }

        [Anônimo, HttpPost("preRegister")]
        public async Task<IActionResult> PreRegistro([FromBody]PreRegisterAgentDto? dto)
        {
            if (dto == null)
            {
                return BadRequest(new RetornoDto("Body nulo"));
            }

            ValidationResult result = await dto.Validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                return BadRequest(new RetornoDto("Body inválido", result.Errors));
            }

            IServiceScope scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var userTentativo = await dbContext.Usuários.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (userTentativo != null)
            {
                return Ok(new RetornoDto("O email informado já está cadastrado no sistema."));
            }

            userTentativo = new UserInfo
            {
                Username = dto.Login!,
                Email = dto.Email!
            };

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            while (await dbContext.Usuários.AnyAsync(x => x.VerificationToken == token))
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

            return Ok(new RetornoDto("Usuário pré-registrado com sucesso!", null));
        }

        [Anônimo, HttpPost("validateToken")]
        public async Task<IActionResult> ValidateToken([FromBody]string? token, [FromBody]string? redirectUrl)
        {
            if (token == null)
            {
                return BadRequest(new RetornoDto("Token inválida"));
            }

            IServiceScope scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var userTentativo = await dbContext.Usuários.FirstOrDefaultAsync(x => x.VerificationToken == token);

            if (userTentativo == null)
            {
                return BadRequest(new RetornoDto("Token inválida"));
            }

            return Redirect($"{redirectUrl}?user={userTentativo.Uuid}");
        }

        [Anônimo, HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterAgentDto? dto)
        {
            if (dto == null)
            {
                return BadRequest(new RetornoDto("Body inválido"));
            }

            if (dto.Uuid == null || string.IsNullOrWhiteSpace(dto.DisplayName) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new RetornoDto("Body incompleto"));
            }

            IServiceScope scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var userTentativo = await dbContext.Usuários.FirstOrDefaultAsync(x => x.Uuid == dto.Uuid);

            if (userTentativo == null)
            {
                return BadRequest(new RetornoDto("Guid inválido"));
            }

            userTentativo.DisplayName = dto.DisplayName;
            userTentativo.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            userTentativo.VerificationToken = null;
            dbContext.Update(userTentativo);
            await dbContext.SaveChangesAsync();

            return Ok(new RetornoDto("Usuário registrado com sucesso!", null));
        }
    }

    public class RefreshApiKeyDto
    {
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("api_key")]
        public string? ApiKey { get; set; }
    }
}
