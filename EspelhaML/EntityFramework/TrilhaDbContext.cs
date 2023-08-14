using EspelhaML.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EspelhaML.EntityFramework
{
    public class TrilhaDbContext : DbContext
    {
        public DbSet<MlUserAuthInfo> MlUserAuthInfos { get; set; } = null!;

        public TrilhaDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if (entity is EntityBase auditableEntity)
            {
                auditableEntity.CreatedAt = DateTime.UtcNow;
                auditableEntity.UpdatedAt = auditableEntity.CreatedAt;
            }
            return base.Add(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            if (entity is EntityBase auditableEntity)
            {
                auditableEntity.UpdatedAt = DateTime.UtcNow;
            }
            return base.Update(entity);
        }
    }
}
