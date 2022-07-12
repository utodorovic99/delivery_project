using DeliveryService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DeliveryService.Data.Configurations
{
  public class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasKey(x => x.Username);

      builder.Property(x => x.Email).IsRequired();
      builder.Property(x => x.Password).IsRequired();
      builder.Property(x => x.Username).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Surname).IsRequired();
      builder.Property(x => x.Birthdate).IsRequired();
      builder.Property(x => x.Address).IsRequired();
      builder.Property(x => x.Type).IsRequired();
      builder.Property(x => x.State).IsRequired();

      builder.HasIndex(x => x.Email).IsUnique();
      
      builder.Property(x => x.Email).HasMaxLength(40);     
      builder.Property(x => x.Password).HasMaxLength(64);    
      builder.Property(x => x.Username).HasMaxLength(25);   
      builder.Property(x => x.Name).HasMaxLength(15);   
      builder.Property(x => x.Surname).HasMaxLength(25);   
      builder.Property(x => x.Birthdate).HasMaxLength(15);
      builder.Property(x => x.Address).HasMaxLength(35);    
      builder.Property(x => x.Type).HasMaxLength(20);     
      builder.Property(x => x.ImageName).HasMaxLength(65);  
    }
  }
}
