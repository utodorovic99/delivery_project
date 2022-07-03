using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Model
{
  public class Ingredient
  {
    public Ingredient(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public Ingredient()
    {
      Id = 0;
      Name = "";
    }

    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

  }
}
