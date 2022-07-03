using DeliveryService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Data.Configurations
{
  public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
  {
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).ValueGeneratedOnAdd();

      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();

      builder.Property(x => x.Name).HasMaxLength(35);
      builder.HasIndex(x => x.Name).IsUnique();
    }
  }
}
