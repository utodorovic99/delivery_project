using DeliveryService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Data.Configurations
{
  public class ProductDefinitionConfiguration : IEntityTypeConfiguration<ProductDefiniton>
  {
    public void Configure(EntityTypeBuilder<ProductDefiniton> builder)
    {
      builder.HasKey(x => new { x.ProductId, x.IngredientId});
      builder.Property(x => x.ProductId).ValueGeneratedOnAdd();

      builder.Property(x => x.ProductId).IsRequired();
      builder.Property(x => x.IngredientId).IsRequired();
    }
  }
}
