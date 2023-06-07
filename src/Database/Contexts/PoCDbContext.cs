using Database.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts
{
    public class PoCDbContext : DbContext
    {
        private readonly DbSet<Customer>? _customers;
        private readonly DbSet<Product>? _products;
        private readonly DbSet<Order>? _orders;
        private readonly DbSet<OrderItem>? _items;
        public PoCDbContext(DbContextOptions<PoCDbContext> options) 
            : base(options)
        {
            this._customers = this.Set<Customer>();
            this._products = this.Set<Product>();
            this._orders = this.Set<Order>();
            this._items = this.Set<OrderItem>();
        }

        public DbSet<Customer> Customers => this._customers!;
        public DbSet<Product> Products => this._products!;
        public DbSet<Order> Orders => this._orders!;
        public DbSet<OrderItem> OrderItems => this._items!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.ApplyConfiguration(new CustomerModelConfiguration());
            _ = modelBuilder.ApplyConfiguration(new ProductModelConfiguration());
            _ = modelBuilder.ApplyConfiguration(new OrderModelConfiguration());
            _ = modelBuilder.ApplyConfiguration(new OrderItemModelConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
