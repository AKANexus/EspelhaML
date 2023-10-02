using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Domain;
using MlSuite.DTOs;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlApiServiceLib;
using MlSuite.MlSynch.Services;

namespace MlSuite.MlSynch.Controllers
{
    [Route("v1/[controller]"), ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceProvider _provider;

        public AuthController(IServiceProvider provider)
        {
            _provider = provider;
        }

        [HttpGet(nameof(MlRedirect))]
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
                    var meInfo = await mlApiService.GetMeInfo(data.AccessToken!);
                    if (meInfo.data is null)
                    {
                        return BadRequest("Falha ao obter os dados do usuário");
                    }
                    var context = scopedProvider.GetRequiredService<TrilhaDbContext>();
                    MlUserAuthInfo tentative =
                        (await context.MlUserAuthInfos.FirstOrDefaultAsync(x => x.UserId == data.UserId)
                         ??
                         new MlUserAuthInfo(data.AccessToken!, DateTime.Now.AddSeconds(data.ExpiresIn ?? 21600), (ulong)data.UserId!,
                             data.RefreshToken!, meInfo.data.Nickname, meInfo.data.Identification.Number)
                        );

                    context.MlUserAuthInfos.Update(tentative);
                    await context.SaveChangesAsync();
                    return Redirect("https://www.mercadolivre.com.br/");
                }
            }
        }


    }
}
