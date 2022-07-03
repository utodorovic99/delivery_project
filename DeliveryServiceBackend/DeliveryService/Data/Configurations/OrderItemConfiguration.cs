using DeliveryService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Data.Configurations
{
  public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
  {
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
      builder.HasKey(x => new { x.OrderId, x.ProductId });
      builder.Property(x => x.OrderId).ValueGeneratedOnAdd();

      builder.Property(x => x.OrderId).IsRequired();
      builder.Property(x => x.ProductId).IsRequired();
      builder.Property(x => x.Quantity).IsRequired();
    }
  }
}
