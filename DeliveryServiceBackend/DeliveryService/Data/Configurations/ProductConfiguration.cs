using DeliveryService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Data.Configurations
{
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).ValueGeneratedOnAdd();

      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Price).IsRequired();

      builder.HasIndex(x => x.Name).IsUnique();
    }
  }
}
