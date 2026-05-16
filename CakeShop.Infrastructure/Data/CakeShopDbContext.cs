using EC.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Infrastructure.Data;

public class CakeShopDbContext : DbContext
{
    public CakeShopDbContext(DbContextOptions<CakeShopDbContext> options) : base(options) { }

    public DbSet<Product>      Products      => Set<Product>();
    public DbSet<Category>     Categories    => Set<Category>();
    public DbSet<User>         Users         => Set<User>();
    public DbSet<CartItem>     CartItems     => Set<CartItem>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<B2eUser>      B2eUsers      => Set<B2eUser>();
    public DbSet<B2eRole>      B2eRoles      => Set<B2eRole>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Price).HasColumnType("numeric(10,2)");
            e.HasOne(p => p.Category)
             .WithMany()
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Category>(e => e.HasKey(c => c.Id));

        builder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Username).IsUnique();
        });

        builder.Entity<CartItem>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasOne(c => c.Product)
             .WithMany()
             .HasForeignKey(c => c.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Announcement>(e => e.HasKey(a => a.Id));

        builder.Entity<B2eRole>(e =>
        {
            e.HasKey(r => r.Id);
            e.ToTable("b2e_roles");
            e.HasIndex(r => r.Name).IsUnique();
        });

        builder.Entity<B2eUser>(e =>
        {
            e.HasKey(u => u.Id);
            e.ToTable("b2e_users");
            e.HasIndex(u => u.Username).IsUnique();
            e.HasOne(u => u.Role)
             .WithMany()
             .HasForeignKey(u => u.RoleId)
             .OnDelete(DeleteBehavior.SetNull);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdateCount++;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
