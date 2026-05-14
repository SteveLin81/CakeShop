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
    }
}
