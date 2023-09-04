using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using EspelhaML.Domain;
using EspelhaML.DTO;
using EspelhaML.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EspelhaML.Services
{
    public class ProcessItemService
    {
        private readonly IServiceProvider _provider;

        public ProcessItemService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task ProcessInfo(string resourceId, string apiToken)
        {
            IServiceProvider scopeProvider = _provider.CreateScope().ServiceProvider;
            MlApiService mlApi = scopeProvider.GetRequiredService<MlApiService>();
            TrilhaDbContext context = scopeProvider.GetRequiredService<TrilhaDbContext>();
            var itemResponse = await mlApi.GetItemById(apiToken, resourceId);
            if (itemResponse.data?.Id is null)
            {
                context.Logs.Add(new EspelhoLog(nameof(ProcessItemService),
                    $"Falha ao obter os dados requisitados: {itemResponse.data?.Error}"));
                await context.SaveChangesAsync();
                return;
            }

            Item? tentativo = await context.Itens
                .Include(item => item.ItemVariação)
                .FirstOrDefaultAsync(x => x.Id == itemResponse.data.Id);
            if (tentativo == null)
            {
                tentativo = new Item(
                    id: itemResponse.data.Id,
                    título: itemResponse.data.Title,
                    sellerId: itemResponse.data.SellerId,
                    category: itemResponse.data.CategoryId,
                    preçoVenda: (decimal)itemResponse.data.Price,
                    quantidadeÀVenda: (itemResponse.data.AvailableQuantity ?? 0),
                    permalink: itemResponse.data.Permalink,
                    primeiraFoto: itemResponse.data.Pictures[0].Url,
                    éVariação: itemResponse.data.Variations.Count > 0
                );
                if (tentativo.ÉVariação)
                {
                    tentativo.ItemVariação
                        .AddRange(itemResponse.data.Variations
                            .Select(x=>new ItemVariação(id:x.Id, preçoVenda:(decimal)(x.Price ?? 0), 
                                descritorVariação:string.Join(' ', x.AttributeCombinations.Select(y=>$"{y.Name}: {y.ValueName}")
                                ))));
                }
            }
            else
            {
                tentativo.Título = itemResponse.data.Title;
                tentativo.Category = itemResponse.data.CategoryId;
                tentativo.PreçoVenda = (decimal)itemResponse.data.Price;
                tentativo.QuantidadeÀVenda = (itemResponse.data.AvailableQuantity ?? 0);
                tentativo.PrimeiraFoto = itemResponse.data.Pictures[0].Url;
                tentativo.ÉVariação = itemResponse.data.Variations.Count > 0;

                if (tentativo.ÉVariação)
                {
                    foreach (Variation variation in itemResponse.data.Variations)
                    {
                        var variaçãoTentativa = tentativo.ItemVariação.FirstOrDefault(x => x.Id == variation.Id);
                        if (variaçãoTentativa is null)
                        {
                            tentativo.ItemVariação.Add(
                                new ItemVariação(variation.Id,
                                    string.Join(' ', variation.AttributeCombinations.Select(y => $"{y.Name}: {y.ValueName}")),
                                (decimal)(variation.Price ?? 0)));
                        }
                        else
                        {
                            variaçãoTentativa.PreçoVenda = (decimal)(variation.Price ?? 0);
                            variaçãoTentativa.DescritorVariação = string.Join(' ',
                                variation.AttributeCombinations.Select(y => $"{y.Name}: {y.ValueName}"));
                        }
                    }

                    foreach (ItemVariação itemVariação in tentativo.ItemVariação)
                    {
                        if (itemResponse.data.Variations.All(x => x.Id != itemVariação.Id))
                        {
                            tentativo.ItemVariação.Remove(itemVariação);
                        }
                    }
                }
                else
                {
                    tentativo.ItemVariação.Clear();
                }
            }
            //Falta fazer uma checagem para atualizações não serem desencadeadas a qualquer notificação do ML

            context.Itens.Update(tentativo);
            await context.SaveChangesAsync();
        }
    }
}
