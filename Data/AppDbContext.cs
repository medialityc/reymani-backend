using System;
using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
