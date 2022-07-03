using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Model
{
  public class OrderItem
  {
    public OrderItem(int orderId, int productId, int quantity)
    {
      OrderId = orderId;
      ProductId = productId;
      Quantity = quantity;
    }

    public OrderItem()
    {
      OrderId = 0;
      ProductId = 0;
      Quantity = 0;
    }

    public int OrderId { get; set; }      //OrderId + ProductId as composite key (assign specific product to specific order)
    public int ProductId { get; set; }
    public int Quantity { get; set; }         //Quantity of products identified by ProductId 
  }
}
