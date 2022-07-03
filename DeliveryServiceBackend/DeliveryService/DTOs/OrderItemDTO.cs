namespace DeliveryService.DTOs
{
  public class OrderItemDTO
  {
    public OrderItemDTO(string name, int quantity, double unitPrice)
    {
      Name = name;
      Quantity = quantity;
      UnitPrice = unitPrice;
    }

    public OrderItemDTO()
    {
      Name = "";
      Quantity = 0;
      UnitPrice = 0.0;
    }

    public string Name { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
  }
}
