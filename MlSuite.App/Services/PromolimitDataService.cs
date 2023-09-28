using Microsoft.EntityFrameworkCore;
using MlSuite.Domain.Entities;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.App.Services
{
    public class PromolimitDataService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PromolimitDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<List<PromolimitEntry>> GetAllProdutos(bool asNoTracking = true)
        {
            using var scope = _scopeFactory.CreateScope();
            return asNoTracking switch
            {
                true => await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                    .PromolimitEntries.Include(x => x.Item)
                    .ThenInclude(y=>y.Seller)
                    .AsNoTracking()
                    .OrderBy(x => x.Item.Id)
                    .ToListAsync(),
                false => await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                    .PromolimitEntries.Include(x => x.Item)
                    .ThenInclude(y=>y.Seller)
                    .OrderBy(x => x.Item.Id)
                    .ToListAsync()
            };
        }

        public async Task<int> CountProdutos()
        {
            using var scope = _scopeFactory.CreateScope();
            return await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                .PromolimitEntries
                .CountAsync();
        }

        public async Task<List<PromolimitEntry>> GetAllProdutosPaged(FilteredQuery filteredQueryModel)
        {
            using var scope = _scopeFactory.CreateScope();
            var request = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>().PromolimitEntries
                .Include(x => x.Item)
                    .ThenInclude(y=>y.Seller)
                .AsNoTracking();
            if (filteredQueryModel.Filters.Length != 0)
            {
                request = request.Where(x => x.Item.Id.Contains(filteredQueryModel.Filters[0].Query));
            }

            return await request
                .OrderBy(x => x.Item.Id)
                .Skip(filteredQueryModel.offset ?? 0)
                .Take(filteredQueryModel.limit ?? 15)
                .ToListAsync();
        }

        public async Task<PromolimitEntry?> AddOrUpdate(PromolimitEntry promolimitEntry)
        {
            using var scope = _scopeFactory.CreateScope();
            var tentativo = await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                .PromolimitEntries.Include(x => x.Item)
                    .ThenInclude(y=>y.Seller)
                .FirstOrDefaultAsync(x => x.Uuid == promolimitEntry.Uuid);
            if (tentativo == null)
            {
                var itemTentativo = await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                    .Itens.FirstOrDefaultAsync(x => x.Id == promolimitEntry.Item.Id);
                if (itemTentativo == null)
                {
                    await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                        .Logs.AddAsync(new("PromolimitDataService.AddOrUpdate",
                            $"O item a ser referenciado não existia: {promolimitEntry.Item.Id}"));
                    return null;
                }
                promolimitEntry.Item = itemTentativo;
                scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                    .PromolimitEntries.Update(promolimitEntry);
            }

            else
            {
                tentativo.QuantidadeAVenda = promolimitEntry.QuantidadeAVenda;
                tentativo.Estoque = promolimitEntry.Estoque;
                scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                    .PromolimitEntries.Update(tentativo);
            }

            await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>().SaveChangesAsync();
            return promolimitEntry;
        }

        public async Task Delete(Guid uuid)
        {
            using var scope = _scopeFactory.CreateScope();
            var tentativo = await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                .PromolimitEntries.FirstOrDefaultAsync(x => x.Uuid == uuid);
            if (tentativo != null)
            {
                scope.ServiceProvider.GetRequiredService<TrilhaDbContext>().PromolimitEntries.Remove(tentativo);
                await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>().SaveChangesAsync();
            }
        }

        public async Task<PromolimitEntry?> GetByMlb(string mlb)
        {
            using var scope = _scopeFactory.CreateScope();
            return await scope.ServiceProvider.GetRequiredService<TrilhaDbContext>()
                .PromolimitEntries.AsNoTracking().Include(x=>x.Item)
                .ThenInclude(y => y.Seller)
                .FirstOrDefaultAsync(x => x.Item.Id == mlb);
        }
    }
}
