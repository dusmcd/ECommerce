using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Models;

namespace ECommerceAPI.Data
{
    public class EcommerceAPIContext : DbContext
    {
        public EcommerceAPIContext(DbContextOptions<EcommerceAPIContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; } = default!;

        public DbSet<Customer> Customers { get; set; } = default!;

        public DbSet<Order> Orders { get; set; } = default!;

        public DbSet<ProductOrder> ProductsOrders { get; set; } = default!;

        public DbSet<ProductCart> ProductsCarts { get; set; } = default!;

        public DbSet<InventoryLog> InventoryLog { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Orders)
                .UsingEntity<ProductOrder>();

            modelBuilder.Entity<Cart>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Carts)
                .UsingEntity<ProductCart>();

            base.OnModelCreating(modelBuilder);
        }

    }
}
