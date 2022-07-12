namespace DeliveryService.DTOs
{
  public class OrderDTO
  {
    public OrderDTO(List<OrderItemDTO> items, double deliveryFee, string address, string comment, char status, int id, string timeExpected)
    {
      Items = items;
      DeliveryFee = deliveryFee;
      Address = address;
      Comment = comment;
      Status = status;
      TimeExpected = timeExpected;
    }

    public OrderDTO()
    {
      Items = new List<OrderItemDTO>();
      DeliveryFee = _delivery_fee;
      Address = "";
      Comment = "";
      Status = 'u';
      Id = Id;
      TimeExpected = "";
  }

    private static double _delivery_fee=0;
    public static void SetConstDeliveryFee(double amount)
    {
      _delivery_fee = amount;
    }

    public List<OrderItemDTO> Items { get; set; }

    public double DeliveryFee { get; set; }

    public string Consumer { get; set; }
    public string Deliveryman { get; set; } 

    public string Address { get; set; }

    public string Comment { get; set; }

    public char Status { get; set; }

    public int Id { get; set; }

    public string TimeExpected { get; set; }
  }
}
