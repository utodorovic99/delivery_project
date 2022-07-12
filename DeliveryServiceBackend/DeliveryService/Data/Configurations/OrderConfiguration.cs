using DeliveryService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Data.Configurations
{
  public class OrderConfiguration : IEntityTypeConfiguration<Order>
  {
    public void Configure(EntityTypeBuilder<Order> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).ValueGeneratedOnAdd();

      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Consumer).IsRequired();
      builder.Property(x => x.Status).IsRequired();
      builder.Property(x => x.Status).IsRequired();
      builder.Property(x => x.Address).IsRequired();

      builder.Property(x => x.Address).HasMaxLength(35);
      builder.Property(x => x.Comment).HasMaxLength(50);
      builder.Property(x => x.Consumer).HasMaxLength(35);
      builder.Property(x => x.Deliveryman).HasMaxLength(35);
      builder.Property(x => x.TimeExpected).HasMaxLength(20);

      builder.Property(x => x.Deliveryman).HasDefaultValue(null);
      builder.Property(x => x.TimeExpected).HasDefaultValue(null);
    }
  }
}
