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

    public AppDbContext()
    {
      
    }

    // DbSet properties
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Municipality> Municipalities { get; set; }
    public virtual DbSet<Province> Provinces { get; set; }
    public virtual DbSet<UserAddress> UserAddresses { get; set; }
    public virtual DbSet<Business> Businesses { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<VehicleType> VehicleTypes { get; set; }
    public virtual DbSet<Vehicle> Vehicles { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ShippingCost> ShippingCosts { get; set; }
    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<ConfirmationNumber> ConfirmationNumbers { get; set; }
    public virtual DbSet<ForgotPasswordNumber> ForgotPasswordNumbers { get; set; }

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