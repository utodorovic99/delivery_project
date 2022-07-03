using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Model
{
  public class Product
  {
    public Product(string name, double price)
    {
      Name = name;
      Price = price;
    }

    public Product()
    {
      Name = "";
      Price = 0.0;
    }

    public int Id { get; set; }                     //Product ID
    public string Name { get; set; } = String.Empty;    //Product Name
    public double Price { get; set; }                   //Produict Price
  }
}
