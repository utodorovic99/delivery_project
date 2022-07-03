using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Model
{
  public class Order
  {
    public Order(int id, string consumer, string deliveryman, char status, string address, string comment)
    {
      Id = id;
      Consumer = consumer;
      Deliveryman = deliveryman;
      Status = Status;
      Address = address;
      Comment = comment;
    }

    public Order()
    {
      Id = 0;
      Consumer = "";
      Deliveryman = "";
      Status = 'u';
      Address = "";
      Comment = "";
    }

    public int Id { get; set; }                             //Unique order ID
    public string Consumer { get; set; }    = String.Empty;     //Cosumer ID (username) who ordered it
    public string Deliveryman { get; set; } = String.Empty;     //Deliveryman ID (null if not taken yet)
    public string Address { get; set; }

    public string Comment { get; set; }
    public char Status { get; set; }                            //Flag if order is available, taken or delivered

  }
}
