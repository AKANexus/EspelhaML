using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlApiServiceLib;

namespace MlSuite.MlSynch.Services
{
    public class ProcessShipmentService
    {
        private readonly IServiceScopeFactory _scopedFactory;

        public ProcessShipmentService(IServiceScopeFactory scopedFactory)
        {
            _scopedFactory = scopedFactory;
        }

        public async Task ProcessInfo(ulong resourceId, string apiToken)
        {
            IServiceProvider scopedProvider = _scopedFactory.CreateScope().ServiceProvider;
            MlApiService mlApi = scopedProvider.GetRequiredService<MlApiService>();
            TrilhaDbContext context = scopedProvider.GetRequiredService<TrilhaDbContext>();
            var shippingResponse = await mlApi.GetShipmentById(apiToken, resourceId.ToString());
            if (shippingResponse.data?.Id is null || shippingResponse.data?.Id == 0)
            {
                context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                    $"Falha ao obter os dados do shipping requisitados (25): {shippingResponse.data?.Error}"));
                await context.SaveChangesAsync();
                return;
            }

            ProcessOrderService processOrderService = scopedProvider.GetRequiredService<ProcessOrderService>();
            if (shippingResponse.data?.OrderId != null)
                await processOrderService.ProcessInfo((ulong)shippingResponse.data.OrderId, apiToken);
        }
    }
}
