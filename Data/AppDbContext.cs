using System;

using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data.Models;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet properties
    public DbSet<User> Users { get; set; }
    public DbSet<Municipality> Municipalities { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShippingCost> ShippingCosts { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ConfirmationNumber> ConfirmationNumbers { get; set; }
    public DbSet<ForgotPasswordNumber> ForgotPasswordNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      foreach (var entry in ChangeTracker.Entries<BaseEntity>()
                   .Where(static x => x.State is EntityState.Added or EntityState.Modified))
      {
        if (entry.State is EntityState.Added)
        {
          entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
          entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
        }
        else if (entry.State == EntityState.Modified)
        {
          entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
        }
      }

      return base.SaveChangesAsync(cancellationToken);
    }
  }
}