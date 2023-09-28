using Microsoft.EntityFrameworkCore;
using MlSuite.Domain;
using MlSuite.Domain.Enums;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlSynch.DTO;

namespace MlSuite.MlSynch.Services
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
            if (orderResponse.data?.Id is null || orderResponse.data.Id == 0)
            {
                context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                    $"Falha ao obter os dados requisitados (27): {orderResponse.data?.Error}"));
                await context.SaveChangesAsync();
                return;
            }

            Pedido? tentativo = await context.Pedidos
                .Include(x => x.Envio)
                .ThenInclude(y => y.Destinatário)
                .Include(x => x.Itens)
                .ThenInclude(pedidoItem => pedidoItem.Item).ThenInclude(item => item.Variações)
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
                        "invalid" => OrderStatus.Ilegal,
                        _ => OrderStatus.Desconhecido
                    },
                    SellerId = orderResponse.data.Seller.Id
                };
                if (orderResponse.data.OrderItems.Count > 0)
                    foreach (OrderItem orderItem in orderResponse.data.OrderItems)
                    {
                        Item? itemTentativo = await context.Itens.Include(y => y.Variações).FirstOrDefaultAsync(y => y.Id == orderItem.Item.Id);
                        if (itemTentativo == null)
                        {
                            var itemResponse = await mlApi.GetItemById(apiToken, orderItem.Item.Id);
                            if (itemResponse.data?.Id is null)
                            {
                                context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                                    $"Falha ao obter os dados requisitados (71): {itemResponse.data?.Error}"));
                                await context.SaveChangesAsync();
                                return;
                            }


                            itemTentativo = new Item(
                                category: itemResponse.data.CategoryId,
                                éVariação: itemResponse.data.Variations.Count > 0,
                                id: itemResponse.data.Id,
                                preçoVenda: (decimal)itemResponse.data.Price,
                                quantidadeÀVenda: (itemResponse.data.AvailableQuantity ?? 0),
                                permalink: itemResponse.data.Permalink,
                                primeiraFoto: itemResponse.data.Pictures[0].Url,
                                título: itemResponse.data.Title
                            );
                            var seller =
                                await context.MlUserAuthInfos.FirstOrDefaultAsync(x =>
                                    x.UserId == itemResponse.data.SellerId);

                            itemTentativo.Seller = seller;

                            if (itemTentativo.ÉVariação)
                            {
                                itemTentativo.Variações
                                    .AddRange(itemResponse.data.Variations
                                        .Select(x => new ItemVariação(id: x.Id, preçoVenda: (decimal)(x.Price ?? 0),
                                            descritorVariação: string.Join(' ', x.AttributeCombinations.Select(y => $"{y.Name}: {y.ValueName}")
                                            ))));
                            }
                        }

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
                if (orderResponse.data.Payments.Count > 0)
                    foreach (Payment payment in orderResponse.data.Payments)
                    {
                        tentativo.Pagamentos.Add(new()
                        {
                            Id = payment.Id,
                            Parcelas = payment.Installments ?? 1,
                            TotalPago = (decimal)(payment.TotalPaidAmount ?? 0),
                            ValorFrete = (decimal)(payment.ShippingCost ?? 0),
                            ValorRessarcido = (decimal)(payment.TransactionAmountRefunded ?? 0),
                            ValorTransação = (decimal)(payment.TransactionAmount ?? 0)
                        });
                    }

                if (orderResponse.data?.Shipping?.Id is not null)
                {
                    var shippingResponse = await mlApi.GetShipmentById(apiToken, orderResponse.data.Shipping.Id.ToString()!);
                    if (shippingResponse.data?.Id is null || shippingResponse.data?.Id == 0)
                    {
                        context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                            $"Falha ao obter os dados do shipping requisitados (128): {shippingResponse.data?.Error}"));
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        tentativo.Envio = new()
                        {
                            Id = shippingResponse.data.Id,
                            Status = shippingResponse.data.Status switch
                            {
                                "pending" => ShipmentStatus.Pendente,
                                "handling" => ShipmentStatus.FretePago,
                                "ready_to_ship" => ShipmentStatus.Autorizado,
                                "shipped" => ShipmentStatus.Enviado,
                                "delivered" => ShipmentStatus.Entregue,
                                "not_delivered" => ShipmentStatus.NãoEntregue,
                                "cancelled" => ShipmentStatus.Cancelado,
                                _ => ShipmentStatus.Desconhecido
                            },
                            SubStatus = shippingResponse.data.Substatus switch
                            {
                                "picked_up" => ShipmentSubStatus.Coletado,
                                "authorized_by_carrier" => ShipmentSubStatus.AutorizadoPelaTransportadora,
                                "in_hub" => ShipmentSubStatus.NoHub,
                                "printed" => ShipmentSubStatus.Impresso,
                                "out_for_delivery" => ShipmentSubStatus.EmRotaEntrega,
                                "returning_to_sender" => ShipmentSubStatus.RetornandoAoVendedor,
                                "fulfilled_feedback" => ShipmentSubStatus.FulfilledFeedback,
                                null => null,
                                _ => ShipmentSubStatus.Desconhecido,
                            },
                            SubStatusDescrição = shippingResponse.data.Substatus,
                            CriaçãoDoPedido = shippingResponse.data.DateCreated,
                            //ValorDeclarado = shippingResponse.data.DeclaredValue,
                            //Largura = shippingResponse.data.Dimensions?.Width,
                            //Altura = shippingResponse.data.Dimensions?.Height,
                            //Comprimento = shippingResponse.data.Dimensions?.Length,
                            //Peso = shippingResponse.data.Dimensions?.Weight,
                            CódRastreamento = shippingResponse.data.TrackingNumber,
                            TipoEnvio = shippingResponse.data.LogisticType switch
                            {
                                "self_service" => ShipmentType.SelfService,
                                "fulfillment" => ShipmentType.Fulfillment,
                                "cross_docking" => ShipmentType.CrossDocking,
                                _ => ShipmentType.Desconhecido
                            },
                            Destinatário = new()
                            {
                                Id = shippingResponse.data.ReceiverAddress.Id,
                                Nome = shippingResponse.data.ReceiverAddress.ReceiverName,
                                Telefone = shippingResponse.data.ReceiverAddress.ReceiverPhone,
                                Logradouro = shippingResponse.data.ReceiverAddress.StreetName,
                                Número = shippingResponse.data.ReceiverAddress.StreetNumber,
                                CEP = shippingResponse.data.ReceiverAddress.ZipCode,
                                Cidade = shippingResponse.data.ReceiverAddress.City.Name,
                                UF = shippingResponse.data.ReceiverAddress.State.Name,
                                Bairro = shippingResponse.data.ReceiverAddress.Neighborhood.Name,
                                Distrito = shippingResponse.data.ReceiverAddress.Municipality?.Name,
                                ÉResidencial = shippingResponse.data.ReceiverAddress.DeliveryPreference == "residential"
                            }
                        };

                    }
                }
            }

            else
            {
                tentativo.Frete = orderResponse.data.ShippingCost;
                tentativo.Status = orderResponse.data.Status switch
                {
                    "confirmed" => OrderStatus.Confirmado,
                    "payment_required" => OrderStatus.PagamentoNecessário,
                    "payment_in_process" => OrderStatus.PagamentoEmProcesso,
                    "partially_paid" => OrderStatus.PagoParcial,
                    "paid" => OrderStatus.Pago,
                    "partially_refunded" => OrderStatus.RessarcidoParcial,
                    "pending_cancel" => OrderStatus.NãoRessarcido,
                    "cancelled" => OrderStatus.Cancelada,
                    "invalid" => OrderStatus.Ilegal,
                    _ => OrderStatus.Desconhecido
                };

                if (orderResponse.data.OrderItems.Count > 0)
                    foreach (OrderItem orderItem in orderResponse.data.OrderItems)
                    {
                        if (tentativo.Itens.All(x => x.Item?.Id != orderItem.Item.Id))
                        {
                            Item? itemTentativo = await context.Itens.Include(y => y.Variações).FirstOrDefaultAsync(y => y.Id == orderItem.Item.Id);
                            if (itemTentativo == null)
                            {
                                var itemResponse = await mlApi.GetItemById(apiToken, orderItem.Item.Id);
                                if (itemResponse.data?.Id is null)
                                {
                                    context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                                        $"Falha ao obter os dados requisitados (219): {itemResponse.data?.Error}"));
                                    await context.SaveChangesAsync();
                                    return;
                                }

                                itemTentativo = new Item(
                                    category: itemResponse.data.CategoryId,
                                    éVariação: itemResponse.data.Variations.Count > 0,
                                    id: itemResponse.data.Id,
                                    preçoVenda: (decimal)itemResponse.data.Price,
                                    quantidadeÀVenda: (itemResponse.data.AvailableQuantity ?? 0),
                                    permalink: itemResponse.data.Permalink,
                                    primeiraFoto: itemResponse.data.Pictures[0].Url,
                                    título: itemResponse.data.Title
                                );
                                var seller =
                                    await context.MlUserAuthInfos.FirstOrDefaultAsync(x =>
                                        x.UserId == itemResponse.data.SellerId);

                                itemTentativo.Seller = seller;
                                if (itemTentativo.ÉVariação)
                                {
                                    itemTentativo.Variações
                                        .AddRange(itemResponse.data.Variations
                                            .Select(x => new ItemVariação(id: x.Id, preçoVenda: (decimal)(x.Price ?? 0),
                                                descritorVariação: string.Join(' ', x.AttributeCombinations.Select(y => $"{y.Name}: {y.ValueName}")
                                                ))));
                                }
                            }
                            
                            else
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

                foreach (PedidoItem pedidoItem in tentativo.Itens)
                {
                    if (orderResponse.data.OrderItems.All(x => x.Item.Id != pedidoItem.Item?.Id))
                    {
                        tentativo.Itens.Remove(pedidoItem);
                    }
                    else
                    {
                        OrderItem orderItem = orderResponse.data.OrderItems.First(x => x.Item.Id == pedidoItem.Item?.Id);
                        pedidoItem.PreçoUnitário = (decimal)(orderItem.UnitPrice ?? 0);
                        pedidoItem.QuantidadeVendida = (orderItem.Quantity ?? 0);
                        pedidoItem.Título = orderItem.Item.Title;
                        pedidoItem.DescritorVariação = string.Join(' ',
                            orderItem.Item.VariationAttributes.Select(y => $"{y.Name}: {y.ValueName}"));
                        if (pedidoItem.Item?.Id != orderItem.Item?.Id && orderItem.Item is not null)
                        {
                            pedidoItem.Item = await context.Itens.Include(y => y.Variações)
                                .FirstOrDefaultAsync(y => y.Id == orderItem.Item.Id);
                        }

                        if (pedidoItem.ItemVariação?.Id != orderItem.Item?.VariationId && orderItem.Item?.VariationId is not null)
                        {
                            pedidoItem.ItemVariação =
                                pedidoItem.Item?.Variações.FirstOrDefault(x => x.Id == orderItem.Item.VariationId);
                        }
                    }
                }
                if (orderResponse.data.Payments.Count > 0)
                    foreach (Payment payment in orderResponse.data.Payments)
                    {
                        if (tentativo.Pagamentos.All(y => y.Id != payment.Id))
                        {
                            tentativo.Pagamentos.Add((new()
                            {
                                Id = payment.Id,
                                Parcelas = payment.Installments ?? 1,
                                TotalPago = (decimal)(payment.TotalPaidAmount ?? 0),
                                ValorFrete = (decimal)(payment.ShippingCost ?? 0),
                                ValorRessarcido = (decimal)(payment.TransactionAmountRefunded ?? 0),
                                ValorTransação = (decimal)(payment.TransactionAmount ?? 0)
                            }));
                        }
                    }

                foreach (PedidoPagamento pagamento in tentativo.Pagamentos)
                {
                    if (orderResponse.data.Payments.All(y => y.Id != pagamento.Id))
                    {
                        tentativo.Pagamentos.Remove(pagamento);
                    }
                    else
                    {
                        Payment payment = orderResponse.data.Payments.First(y => y.Id == pagamento.Id);
                        pagamento.Parcelas = payment.Installments ?? 1;
                        pagamento.Parcelas = payment.Installments ?? 1;
                        pagamento.TotalPago = (decimal)(payment.TotalPaidAmount ?? 0);
                        pagamento.ValorFrete = (decimal)(payment.ShippingCost ?? 0);
                        pagamento.ValorRessarcido = (decimal)(payment.TransactionAmountRefunded ?? 0);
                        pagamento.ValorTransação = (decimal)(payment.TransactionAmount ?? 0);
                    }
                }

                if (orderResponse.data?.Shipping?.Id is not null)
                {
                    var shippingResponse = await mlApi.GetShipmentById(apiToken, orderResponse.data.Shipping.Id.ToString()!);
                    if (shippingResponse.data?.Id is null)
                    {
                        context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                            $"Falha ao obter os dados do shipping requisitados (295): {shippingResponse.data?.Error}"));
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        if (tentativo.Envio is null)
                        {
                            tentativo.Envio = new()
                            {
                                Id = shippingResponse.data.Id,
                                Status = shippingResponse.data.Status switch
                                {
                                    "pending" => ShipmentStatus.Pendente,
                                    "handling" => ShipmentStatus.FretePago,
                                    "ready_to_ship" => ShipmentStatus.Autorizado,
                                    "shipped" => ShipmentStatus.Enviado,
                                    "delivered" => ShipmentStatus.Entregue,
                                    "not_delivered" => ShipmentStatus.NãoEntregue,
                                    "cancelled" => ShipmentStatus.Cancelado,
                                    _ => ShipmentStatus.Desconhecido
                                },
                                SubStatus = shippingResponse.data.Substatus switch
                                {
                                    "picked_up" => ShipmentSubStatus.Coletado,
                                    "authorized_by_carrier" => ShipmentSubStatus.AutorizadoPelaTransportadora,
                                    "in_hub" => ShipmentSubStatus.NoHub,
                                    "printed" => ShipmentSubStatus.Impresso,
                                    _ => ShipmentSubStatus.Impresso,
                                },
                                SubStatusDescrição = shippingResponse.data.Substatus,
                                CriaçãoDoPedido = shippingResponse.data.DateCreated,
                                //ValorDeclarado = shippingResponse.data.DeclaredValue,
                                //Largura = shippingResponse.data.Dimensions?.Width,
                                //Altura = shippingResponse.data.Dimensions?.Height,
                                //Comprimento = shippingResponse.data.Dimensions?.Length,
                                //Peso = shippingResponse.data.Dimensions?.Weight,
                                CódRastreamento = shippingResponse.data.TrackingNumber,
                                Destinatário = new()
                                {
                                    Id = shippingResponse.data.ReceiverAddress.Id,
                                    Nome = shippingResponse.data.ReceiverAddress.ReceiverName ?? shippingResponse.data.ReceiverAddress?.Agency?.Description ?? "N/A",
                                    Telefone = shippingResponse.data.ReceiverAddress.ReceiverPhone ?? shippingResponse.data.ReceiverAddress?.Agency?.Phone ?? "XXXX",
                                    Logradouro = shippingResponse.data.ReceiverAddress.StreetName,
                                    Número = shippingResponse.data.ReceiverAddress.StreetNumber,
                                    CEP = shippingResponse.data.ReceiverAddress.ZipCode,
                                    Cidade = shippingResponse.data.ReceiverAddress.City.Name,
                                    UF = shippingResponse.data.ReceiverAddress.State.Name,
                                    Bairro = shippingResponse.data.ReceiverAddress.Neighborhood.Name,
                                    Distrito = shippingResponse.data.ReceiverAddress.Municipality.Name,
                                    ÉResidencial = shippingResponse.data.ReceiverAddress.DeliveryPreference == "residential"
                                }
                            };
                        }
                        else
                        {
                            tentativo.Envio.Status = shippingResponse.data.Status switch
                            {
                                "pending" => ShipmentStatus.Pendente,
                                "handling" => ShipmentStatus.FretePago,
                                "ready_to_ship" => ShipmentStatus.Autorizado,
                                "shipped" => ShipmentStatus.Enviado,
                                "delivered" => ShipmentStatus.Entregue,
                                "not_delivered" => ShipmentStatus.NãoEntregue,
                                "cancelled" => ShipmentStatus.Cancelado,
                                _ => ShipmentStatus.Desconhecido
                            };
                            tentativo.Envio.SubStatus = shippingResponse.data.Substatus switch
                            {
                                "picked_up" => ShipmentSubStatus.Coletado,
                                "authorized_by_carrier" => ShipmentSubStatus.AutorizadoPelaTransportadora,
                                "in_hub" => ShipmentSubStatus.NoHub,
                                "printed" => ShipmentSubStatus.Impresso,
                                _ => ShipmentSubStatus.Impresso,
                            };
                            tentativo.Envio.SubStatusDescrição = shippingResponse.data.Substatus;
                            //tentativo.Envio.ValorDeclarado = shippingResponse.data.DeclaredValue;
                            //tentativo.Envio.Largura = shippingResponse.data.Dimensions?.Width;
                            //tentativo.Envio.Altura = shippingResponse.data.Dimensions?.Height;
                            //tentativo.Envio.Comprimento = shippingResponse.data.Dimensions?.Length;
                            //tentativo.Envio.Peso = shippingResponse.data.Dimensions?.Weight;
                            tentativo.Envio.CódRastreamento = shippingResponse.data.TrackingNumber;
                            tentativo.Envio.Destinatário.Nome = shippingResponse.data.ReceiverAddress.ReceiverName ?? shippingResponse.data.ReceiverAddress?.Agency?.Description ?? "N/A";
                            tentativo.Envio.Destinatário.Telefone = shippingResponse.data.ReceiverAddress.ReceiverPhone ?? shippingResponse.data.ReceiverAddress?.Agency?.Phone ?? "XXXX";
                            tentativo.Envio.Destinatário.Logradouro = shippingResponse.data.ReceiverAddress.StreetName;
                            tentativo.Envio.Destinatário.Número = shippingResponse.data.ReceiverAddress.StreetNumber;
                            tentativo.Envio.Destinatário.CEP = shippingResponse.data.ReceiverAddress.ZipCode;
                            tentativo.Envio.Destinatário.Cidade = shippingResponse.data.ReceiverAddress.City.Name;
                            tentativo.Envio.Destinatário.UF = shippingResponse.data.ReceiverAddress.State.Name;
                            tentativo.Envio.Destinatário.Bairro = shippingResponse.data.ReceiverAddress.Neighborhood.Name;
                            tentativo.Envio.Destinatário.Distrito = shippingResponse.data.ReceiverAddress.Municipality.Name;
                            tentativo.Envio.Destinatário.ÉResidencial = shippingResponse.data.ReceiverAddress.DeliveryPreference ==
                                                                        "residential";
                        }
                    }
                }
            }

            context.Pedidos.Update(tentativo);
            await context.SaveChangesAsync();
        }
    }
}
