using EspelhaML.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EspelhaML.EntityFramework
{
    public class TrilhaDbContext : DbContext
    {
        public DbSet<MlUserAuthInfo> MlUserAuthInfos { get; set; } = null!;
        public DbSet<EspelhoLog> Logs { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;

        public TrilhaDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public override int SaveChanges()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = now;
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = now;
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        //public override EntityEntry Add(object entity)
        //{
        //    if (entity is EntityBase auditableEntity)
        //    {
        //        auditableEntity.CreatedAt = DateTime.Now;
        //        auditableEntity.UpdatedAt = auditableEntity.CreatedAt;
        //    }
        //    return base.Add(entity);
        //}

        //public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        //{
        //    if (entity is EntityBase auditableEntity)
        //    {
        //        auditableEntity.CreatedAt = DateTime.Now;
        //        auditableEntity.UpdatedAt = auditableEntity.CreatedAt;
        //    }
        //    return base.Add(entity);
        //}

        //public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        //{
        //    if (entity is EntityBase auditableEntity)
        //    {
        //        auditableEntity.CreatedAt = DateTime.Now;
        //        auditableEntity.UpdatedAt = auditableEntity.CreatedAt;
        //    }
        //    return base.AddAsync(entity, cancellationToken);
        //}

        //public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        //{
        //    if (entity is EntityBase auditableEntity)
        //    {
        //        if (auditableEntity.CreatedAt == default)
        //        {
        //            auditableEntity.CreatedAt = DateTime.Now;
        //        }
        //        auditableEntity.UpdatedAt = DateTime.Now;
        //    }
        //    return base.Update(entity);
        //}

    }
}
