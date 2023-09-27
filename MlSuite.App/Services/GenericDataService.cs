using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MlSuite.Domain;
using MlSuite.Domain.Entities;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.App.Services
{
    public class GenericDataService<TEntity> where TEntity : EntityBase
    {
        private readonly TrilhaDbContext _context;

        public GenericDataService(TrilhaDbContext context)
        {
            _context = context;
        }

        public async Task<(List<TEntity>? results, int pages, int totalRecords)> GetWithFilteringPagingAsync(
            FilteredQuery filteredQuery, Guid tenant)
        {
            filteredQuery.limit ??= 30;
            if (filteredQuery.limit > 30) filteredQuery.limit = 30;
            filteredQuery.offset ??= 0;
            var dbQuery = _context.Set<TEntity>().ApplyFiltering(filteredQuery);
            if (dbQuery == null)
            {
                return (null, 0, 0);
            }

            //CustomerProfile? tenantProfile = await _context.Set<CustomerProfile>().FirstOrDefaultAsync(x => x.Uuid == tenant);
            //if (tenantProfile == null)
            //{
            //    return (null, 0, 0);
            //}
            //dbQuery = dbQuery.Where(x => x.Tenant == tenantProfile);
            var totalRecords = await dbQuery.CountAsync();
            var pages = Math.Ceiling(totalRecords / (decimal)filteredQuery.limit);
            var result = await dbQuery.Skip((int)filteredQuery.offset).Take((int)filteredQuery.limit).ToListAsync();
            return (result, (int)pages, totalRecords);

        }

        public async Task<TEntity?> GetByUuid(Guid uuid, Guid tenant, params Expression<Func<TEntity, object>>[] includes)
        {
            return await includes.Aggregate((IQueryable<TEntity>)_context.Set<TEntity>(),
                    (current,
                        next) => current.Include(next))
                //.Include(x => x.Tenant)
                .FirstOrDefaultAsync(x => x.Uuid == uuid
                                          //&& x.Tenant.Uuid == tenant
                                          );
        }

        public async Task<TEntity?> GetByUuid(Guid uuid, Guid tenant, params string[] includes)
        {
            return await includes.Aggregate((IQueryable<TEntity>)_context.Set<TEntity>(),
                    (current,
                        next) => current.Include(next))
                //.Include(x => x.Tenant)
                .FirstOrDefaultAsync(x => x.Uuid == uuid 
                                          //&& x.Tenant.Uuid == tenant
                                          );
        }

        public async Task<TEntity> AddOrUpdate(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> GetByUuid(Guid uuid, Guid tenant)
        {
            return await _context.Set<TEntity>()
                //.Include(x => x.Tenant)
                .FirstOrDefaultAsync(x => x.Uuid == uuid
                                          //&& x.Tenant.Uuid == tenant
                                          );
        }
    }
}
