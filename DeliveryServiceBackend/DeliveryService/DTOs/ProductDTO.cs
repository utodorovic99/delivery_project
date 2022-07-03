namespace DeliveryService.DTOs
{
  public class ProductDTO
  {
    public ProductDTO(string name, double price, List<string> ingredients)
    {
      Name = name;
      Price = price;
      Ingredients = ingredients;
    }

    public ProductDTO()
    {
      Name = "";
      Price = 0.0;
      Ingredients = new List<string>();
    }

    public string Name { get; set; }
    public double Price { get; set; }

    public List<string> Ingredients { get; set; }
  }
}
