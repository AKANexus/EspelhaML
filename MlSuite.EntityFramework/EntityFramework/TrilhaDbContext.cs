using Microsoft.EntityFrameworkCore;
using MlSuite.Domain;

namespace MlSuite.EntityFramework.EntityFramework
{
    public class TrilhaDbContext : DbContext
    {
        public DbSet<MlUserAuthInfo> MlUserAuthInfos { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<EspelhoLog> Logs { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Item> Itens { get; set; } = null!;
        public DbSet<Pedido> Pedidos { get; set; } = null!;
        public DbSet<UserInfo> Usuários { get; set; } = null!;
        public DbSet<Separação> Separações { get; set; } = null!;
        public DbSet<PromolimitEntry> PromolimitEntries { get; set; } = null!;

        public TrilhaDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasAlternateKey(x => x.Id);
            modelBuilder.Entity<Envio>()
                .HasAlternateKey(x => x.Id);
            modelBuilder.Entity<PedidoPagamento>()
                .HasAlternateKey(x => x.Id);
            modelBuilder.Entity<Item>()
                .HasAlternateKey(x => x.Id);
            modelBuilder.Entity<ItemVariação>()
                .HasAlternateKey(x => x.Id);
            modelBuilder.Entity<Question>()
                .HasAlternateKey(x => x.Id);
            modelBuilder.Entity<MlUserAuthInfo>()
                .HasAlternateKey(x => x.UserId);
            modelBuilder.Entity<Item>()
                .HasOne(item => item.Seller)
                .WithMany();
            modelBuilder.Entity<Pedido>()
                .HasOne(x => x.Separação)
                .WithMany(y => y.Pedidos);

            base.OnModelCreating(modelBuilder);
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
    }
}
