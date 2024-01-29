using Microsoft.EntityFrameworkCore;
using MlSuite.Api.DTOs;
using MlSuite.Domain;
using MlSuite.Domain.Enums;
using MlSuite.EntityFramework.EntityFramework;
using RestSharp;

namespace MlSuite.Api.Services
{
    public class WebHookService
    {
        private IServiceScopeFactory _scopeFactory;

        public WebHookService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task NotificaWebhooks(WebHookTopic topic, Guid uuid)
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();
            List<WebHookInfo> webHookSubscribers = await context.WebhookSubscribers.ToListAsync();
            var tasks = webHookSubscribers.Where(x=>x.WebHookTopic == topic).Select(x => Task.Run(async () =>
            {
                using RestClient client = new RestClient(x.CallbackUrl);
                RestRequest webhookRequest = new RestRequest(x.CallbackUrl).AddBody(new WebHookCallDto(DateTime.Now, topic, uuid));
                await client.ExecutePostAsync<dynamic>(webhookRequest);
            }));
            await Task.WhenAll(tasks);
        }
    }
}
