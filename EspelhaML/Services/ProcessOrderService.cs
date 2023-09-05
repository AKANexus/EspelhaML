using EspelhaML.Domain;
using EspelhaML.Domain.Enums;
using EspelhaML.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EspelhaML.Services
{
    public class ProcessOrderService
    {
        private readonly IServiceProvider _provider;


        public ProcessOrderService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task ProcessInfo(ulong resourceId, string apiToken)
        {
            IServiceProvider scopedProvider = _provider.CreateScope().ServiceProvider;
            MlApiService mlApi = scopedProvider.GetRequiredService<MlApiService>();
            TrilhaDbContext context = scopedProvider.GetRequiredService<TrilhaDbContext>();
            var orderResponse = await mlApi.GetOrderById(apiToken, resourceId.ToString());
            if (orderResponse.data?.Id is null)
            {
                context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                    $"Falha ao obter os dados requisitados: {orderResponse.data?.Error}"));
                await context.SaveChangesAsync();
                return;
            }

            Pedido? tentativo = await context.Pedidos
                .Include(x => x.Envio)
                .ThenInclude(y => y.Destinatário)
                .Include(x => x.Itens)
                .Include(x => x.Pagamentos)
                .FirstOrDefaultAsync(x => x.Id == orderResponse.data.Id);

            if (tentativo == null)
            {
                tentativo = new()
                {
                    Id = orderResponse.data.Id,
                    Frete = orderResponse.data.ShippingCost,
                    Status = orderResponse.data.Status switch
                    {
                        "confirmed" => OrderStatus.Confirmado,
                        "payment_required" => OrderStatus.PagamentoNecessário,
                        "payment_in_process" => OrderStatus.PagamentoEmProcesso,
                        "partially_paid" => OrderStatus.PagoParcial,
                        "paid" => OrderStatus.Pago,
                        "partially_refunded" => OrderStatus.RessarcidoParcial,
                        "pending_cancel" => OrderStatus.NãoRessarcido,
                        "cancelled" => OrderStatus.Cancelada,
                        "invalid" => OrderStatus.Ilegal
                    }
                };
                if (orderResponse.data.OrderItems.Count > 0)
                    foreach (OrderItem orderItem in orderResponse.data.OrderItems)
                    {
                        Item? itemTentativo = await context.Itens.Include(y => y.Variações).FirstOrDefaultAsync(y => y.Id == orderItem.Item.Id);
                        if (itemTentativo != null)
                        {
                            tentativo.Itens.Add(new PedidoItem
                            {
                                Item = itemTentativo,
                                ItemVariação = itemTentativo.Variações.FirstOrDefault(x => x.Id == orderItem.Item.VariationId),
                                DescritorVariação = string.Join(' ', orderItem.Item.VariationAttributes.Select(y => $"{y.Name}: {y.ValueName}")),
                                PreçoUnitário = (decimal)(orderItem.UnitPrice ?? 0),
                                QuantidadeVendida = (orderItem.Quantity ?? 0),
                                Título = orderItem.Item.Title
                            });
                        }
                    }
            }
        }
    }
}
