using DeliveryService.Model;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data
{
  public class DataContext:DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
  }
}
