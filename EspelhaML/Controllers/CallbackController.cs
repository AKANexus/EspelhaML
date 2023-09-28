using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlSynch.DTO;
using MlSuite.MlSynch.Services;

namespace MlSuite.MlSynch.Controllers
{
    [Route(""),ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IServiceScopeFactory _resolver;
        //private readonly IServiceProvider _provider;

        public CallbackController(IServiceScopeFactory resolver)
        {
            _resolver = resolver;
            //_provider = provider;
            CallbackReceived += async (_, args) => await ProcessCallback(args.NotificationDto);
        }

        private event CallBackEventHandler? CallbackReceived;
        private delegate void CallBackEventHandler(object sender,  CallBackEventArgs args);

        private async Task<string?> GetAccessTokenByUserId(ulong userId)
        {
            IServiceProvider scopedProvider = _resolver.CreateScope().ServiceProvider;
            TrilhaDbContext context = scopedProvider.GetRequiredService<TrilhaDbContext>();
            MlApiService mlApi = scopedProvider.GetRequiredService<MlApiService>();

            MlUserAuthInfo? accessToken =
                (await context.MlUserAuthInfos.FirstOrDefaultAsync(x => x.UserId == userId));
            if (accessToken == null)
            {
                context.Logs.Add(new EspelhoLog(nameof(GetAccessTokenByUserId), $"A access token do userId {userId} não pôde ser obtida."));
                await context.SaveChangesAsync();
                return null;
            }

            if (accessToken.IsExpired)
            {
                (int status, AccessTokenDto? data) newAccessToken = await mlApi.RefreshAccessToken(accessToken.RefreshToken);
                if (newAccessToken.data?.AccessToken is null)
                {
                    context.Logs.Add(new EspelhoLog(nameof(GetAccessTokenByUserId), $"Falha ao obter uma nova access token: {newAccessToken.data?.Error}"));
                    await context.SaveChangesAsync();
                    return null;
                }
                else
                {
                    if (newAccessToken.data.RefreshToken is null)
                    {
                        context.Logs.Add(new EspelhoLog(nameof(GetAccessTokenByUserId), $"Falha ao obter uma nova access token - refresh token estava nulo"));
                        await context.SaveChangesAsync();
                        return null;
                    }
                    accessToken.ExpiresOn = DateTime.Now.AddSeconds(newAccessToken.data.ExpiresIn ?? 21600);
                    accessToken.AccessToken = newAccessToken.data.AccessToken;
                    accessToken.RefreshToken = newAccessToken.data.RefreshToken;

                    (int status, MeDto? data) meData = await mlApi.GetMeInfo(accessToken.AccessToken);
                    if (meData.data != null)
                    {
                        accessToken.AccountNickname = meData.data.Nickname;
                        accessToken.AccountRegistry = meData.data.Identification.Number;
                    }
                    
                    context.Update(accessToken);
                    await context.SaveChangesAsync();
                    return accessToken.AccessToken;
                }
            }
            else
            {
                return accessToken.AccessToken;
            }
        }

        private async Task ProcessCallback(NotificationDto notification)
        {
            //Debug.WriteLine("==========Yellow light!==========");
            await CallbackSemaphore.semaphore.WaitAsync();
            //Debug.WriteLine("==========Green light!==========");
            IServiceProvider scopedProvider = _resolver.CreateScope().ServiceProvider;
            TrilhaDbContext context = scopedProvider.GetRequiredService<TrilhaDbContext>();
            string? accessToken = await GetAccessTokenByUserId(notification.UserId);
            if (accessToken == null) return;
            ulong.TryParse(notification.Resource.Split('/').Last(), out var resourceId);
            switch (notification.Topic)
            {
                case "questions":
                    if (resourceId == 0)
                    {
                        context.Logs.Add(new EspelhoLog(nameof(ProcessCallback),
                            $"Falha ao obter resourceId de {notification.Resource}"));
                        await context.SaveChangesAsync();
                        return;
                    }
                    ProcessQuestionService processQuestionService = scopedProvider.GetRequiredService<ProcessQuestionService>();
                  await processQuestionService.ProcessInfo(resourceId, accessToken);
                    break;
                case "items":
                    ProcessItemService processItemService = scopedProvider.GetRequiredService<ProcessItemService>();
                    await processItemService.ProcessInfo(notification.Resource.Split('/').Last(), accessToken);
                    break;
                case "orders_v2":
                    ProcessOrderService processOrderService = scopedProvider.GetRequiredService<ProcessOrderService>();
                    await processOrderService.ProcessInfo(resourceId, accessToken);
                    break;
                default:
                    break;
            }

            //await Task.Delay(10000);
            CallbackSemaphore.semaphore.Release();
        }

        [HttpPost("MlCallback")]
        public IActionResult MlCallback([FromBody] NotificationDto notificationDto)
        {
            CallbackReceived?.Invoke(this, new CallBackEventArgs(notificationDto));
            return Ok();
        }
    }

    public class CallBackEventArgs : EventArgs
    {
        public CallBackEventArgs(NotificationDto notificationDto)
        {
            NotificationDto = notificationDto;
        }

        public NotificationDto NotificationDto { get; set; }
    }
}
