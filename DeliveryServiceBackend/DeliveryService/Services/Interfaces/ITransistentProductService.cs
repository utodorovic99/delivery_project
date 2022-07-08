using DeliveryService.DTOs;

namespace DeliveryService.Services.Interfaces
{
  public interface ITransistentProductService
  {
    public bool PublishOrder(OrderDTO order, out string errMsg);

    public bool ConfirmDelivery(int orderId, out string errMsg);

    public List<OrderDTO> GetAllOrders();

    public List<OrderItemDTO> GetOrderItems(int orderId, out string errMsg);

    public List<OrderDTO> ConfirmedOrdersFor(string deliverymanUsername, out string errMsg);

    public List<OrderDTO> CompletedOrdersFor(string deliverymanUsername, out string errMsg);

    public List<OrderDTO> AvailableOrdersFor(string deliverymanUsername, out string errMsg);

    public string AcceptOrder(int orderId, string deliverymanUsername, out string errStr);

    public bool CreateProduct(ProductDTO product, out string errMsg);
  }
}
