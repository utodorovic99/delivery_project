using DeliveryService.Model;
using DeliveryService.Services;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data
{
  public class DeliveryDataContext : DbContext
  {
    public DeliveryDataContext(DbContextOptions<DeliveryDataContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      var users       = modelBuilder.Entity<User>();
      var ingredients = modelBuilder.Entity<Ingredient>();
      var orders      = modelBuilder.Entity<Order>();
      var orderItems  = modelBuilder.Entity<OrderItem>();
      var products    = modelBuilder.Entity <Product>();
      var productDefs = modelBuilder.Entity <ProductDefiniton>();
      
      users.ToTable("Users");
      ingredients.ToTable( "Ingredients" );
      orders.ToTable( "Orders");
      orderItems.ToTable( "OrderItems" );
      products.ToTable( "Products");
      productDefs.ToTable( "ProductDefinitions");

      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryDataContext).Assembly);
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductDefiniton> ProductDefinitions => Set<ProductDefiniton>();
  }
}
