using EspelhaML.Domain;
using EspelhaML.DTO;
using EspelhaML.EntityFramework;
using EspelhaML.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EspelhaML.Controllers
{
    [Route("v1/[controller]"), ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceProvider _provider;

        public AuthController(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<IActionResult> MlRedirect([FromQuery] string code)
        {
            using IServiceScope serviceScope = _provider.CreateScope();
            IServiceProvider scopedProvider = serviceScope.ServiceProvider;
            var mlApiService = scopedProvider.GetRequiredService<MlApiService>();
            (var status, AccessTokenDto? data) = await mlApiService.ExchangeCodeForFirstLogin(code);

            if (data == null)
            {
                return BadRequest(status);
            }
            else
            {
                if (!string.IsNullOrEmpty(data.Error))
                {
                    return BadRequest();
                }
                else
                {

                    var context = scopedProvider.GetRequiredService<TrilhaDbContext>();
                    MlUserAuthInfo tentative =
                        (await context.MlUserAuthInfos.FirstOrDefaultAsync(x => x.UserId == data.UserId)
                         ??
                         new MlUserAuthInfo(data.AccessToken!, DateTime.UtcNow.AddSeconds(data.ExpiresIn ?? 21600), (long)data.UserId!,
                             data.RefreshToken!)
                        );

                    context.MlUserAuthInfos.Update(tentative);
                    await context.SaveChangesAsync();
                    return Redirect("https://www.mercadolivre.com.br/");
                }
            }
        }


    }
}
