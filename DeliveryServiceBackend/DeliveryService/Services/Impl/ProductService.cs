using AutoMapper;
using DeliveryService.Data;
using DeliveryService.DTOs;
using DeliveryService.Model;
using DeliveryService.Services.Interfaces;

namespace DeliveryService.Services.Impl
{
  public class ProductService:ITransistentProductService
  {
    private readonly IMapper _mapper;
    private readonly DeliveryDataContext _dbContext;
    private static readonly double _delivery_fee = 350;

    public ProductService(IMapper mapper, DeliveryDataContext dbContext)
    {
      this._dbContext = dbContext;
      this._mapper = mapper;
      OrderDTO.SetConstDeliveryFee(350);
    }

    public string AcceptOrder(int orderId, string deliverymanUsername, out string errMsg)
    {
      var deliveryman = _dbContext.Users.Find(deliverymanUsername);
      if (deliveryman==null)
      { errMsg = "Invalid useranme."; return $"ERR:{errMsg}"; }

      if (!deliveryman.Type.Equals("d"))
      { errMsg = "Invalid user type."; return $"ERR:{errMsg}"; }

      if(_dbContext.Orders.FirstOrDefault(x=>x.Deliveryman.Equals(deliverymanUsername) &&
                                             x.Status.Equals('a'))!=null)
      { errMsg = "Maximum deliveries reached."; return $"ERR:{errMsg}"; }

      var order = _dbContext.Orders.Find(orderId);
      if (order==null)
      { errMsg = "Invalid order number."; return $"ERR:{errMsg}"; }

      order.Status = 't';
      order.Deliveryman = deliverymanUsername;
      _dbContext.SaveChangesAsync();

      errMsg = "";
      return $"CDWN:{GenerateOrderCountdownMoment().ToShortTimeString()}";

    }

    private DateTime GenerateOrderCountdownMoment()
    {
      var now = DateTime.Now;
      Random r = new Random();
      now.AddMinutes(r.Next(1, 10));
      now.AddMinutes(r.Next(0, 60));
      return now;
    }

    public List<OrderDTO> AvailableOrdersFor(string deliverymanUsername, out string errMsg)
    {
      errMsg = "";
      var orders = new List<OrderDTO>();
      var deliveryman = _dbContext.Users.Find(deliverymanUsername);
      if (deliveryman == null)
      { errMsg = "Invalid useranme."; return orders; }

      if (!deliveryman.Type.Equals("d"))
      { errMsg = "Invalid user type."; return orders; }

      if (_dbContext.Orders.FirstOrDefault(x => x.Deliveryman.Equals(deliverymanUsername) &&
                                             x.Status.Equals('a')) != null)
      { errMsg = "Maximum deliveries reached."; return orders; }

      orders =_mapper.Map<List<OrderDTO>>(_dbContext.Orders.ToList().Where(x => x.Status.Equals('a') &&
                                                                          (x.Deliveryman == null || x.Deliveryman == "")));
      AttachItemsToOrder(ref orders);
      return orders;
    }

    public List<OrderDTO> CompletedOrdersFor(string deliverymanUsername, out string errMsg) //For customer
    {
      errMsg = "";
      var orders = new List<OrderDTO>();
      var deliveryman = _dbContext.Users.Find(deliverymanUsername);
      if (deliveryman == null)
      { errMsg = "Invalid useranme."; return orders; }

      if (!deliveryman.Type.Equals("c"))
      { errMsg = "Invalid user type."; return orders; }

      orders = _mapper.Map<List<OrderDTO>>(_dbContext.Orders.ToList().Where(x => x.Status.Equals('c') &&
                                                                          x.Consumer.Equals(deliverymanUsername)));

      AttachItemsToOrder(ref orders);
      return orders;
    }

    public bool ConfirmDelivery(int orderId, string deliverymanUsername, out string errMsg)
    {
      errMsg = "";
      var deliveryman = _dbContext.Users.Find(deliverymanUsername);
      if (deliveryman == null)
      { errMsg = "Invalid useranme."; return false; }

      if (!deliveryman.Type.Equals("d"))
      { errMsg = "Invalid user type."; return false; }

      var order = _dbContext.Orders.Find(orderId);
      if (order == null)
      { errMsg = "Invalid order number."; return false; }

      if(!order.Deliveryman.Equals(deliverymanUsername))
      { errMsg = "Order taken by another deliveryman"; return false; }

      order.Status = 'c';
      _dbContext.SaveChangesAsync();

      return true;
    }

    public List<OrderDTO> ConfirmedOrdersFor(string deliverymanUsername, out string errMsg)  //For deliveryman
    {
      errMsg = "";
      var orders = new List<OrderDTO>();
      var deliveryman = _dbContext.Users.Find(deliverymanUsername);
      if (deliveryman == null)
      { errMsg = "Invalid useranme."; return orders; }

      if (!deliveryman.Type.Equals("d"))
      { errMsg = "Invalid user type."; return orders; }

      orders = _mapper.Map<List<OrderDTO>>(_dbContext.Orders.ToList().Where(x => x.Status.Equals('c') &&
                                                                            x.Deliveryman.Equals(deliverymanUsername)));
      AttachItemsToOrder(ref orders);

      return orders;
    }

    public bool CreateProduct(ProductDTO product, out string errMsg)
    {
      errMsg = "";
      {
        Dictionary<string, string> errs = new Dictionary<string, string>();
        if (!ProductValidator.ValidateNewProductParams(_dbContext, product, out errs))
        {
          ProductValidator.ErrCollectionToString(errs, out errMsg);
          return false;
        }
      }

      var productModel = _mapper.Map<Product>(product);

      _dbContext.Products.Add(productModel);
      _dbContext.SaveChanges();
      productModel = _dbContext.Products.FirstOrDefault(x => x.Name.Equals(productModel));
      Ingredient ingredientModel = null;
      foreach (var ingredient in product.Ingredients)
      {
        ingredientModel = _dbContext.Ingredients.FirstOrDefault(x => x.Name.Equals(ingredient));
        _dbContext.ProductDefinitions.Add(new ProductDefiniton(productModel.Id, ingredientModel.Id));
      }
      _dbContext.SaveChangesAsync();
      return true;
    }

    public List<OrderDTO> GetAll()
    {
      var orders = _mapper.Map<List<OrderDTO>>(_dbContext.Orders.ToList());
      AttachItemsToOrder(ref orders);
      return orders;
    }

    public bool PublishOrder(OrderDTO order, out string errMsg)
    {
      errMsg = "";
      {
        Dictionary<string, string> errs = new Dictionary<string, string>();
        if (!ProductValidator.ValidateNewOrderParams(_dbContext, order, out errs))
        {
          ProductValidator.ErrCollectionToString(errs, out errMsg);
          return false;
        }
      }      

      var orderModel = _mapper.Map<Order>(order);
      orderModel.Status = 'a';
      orderModel.Deliveryman = "";
      orderModel = (_dbContext.Orders.Add(orderModel)).Entity;
      _dbContext.SaveChanges();

      foreach (var orderItem in order.Items)
      {
        _dbContext.OrderItems.Add(new OrderItem(orderModel.Id,
                                                _dbContext.Products.FirstOrDefault(x => x.Name.Equals(orderItem.Name)).Id,
                                                orderItem.Quantity));
      }

      _dbContext.SaveChangesAsync();
      return true;
    }

    private void AttachItemsToOrder(ref List<OrderDTO> orders)
    {
      OrderDTO orderIt = null;
      Product product = null;
      foreach (var orderItem in _dbContext.OrderItems)
      {
        if ((orderIt = orders.FirstOrDefault(x => x.Id.Equals(orderItem.OrderId))) != null)
        {
          product = _dbContext.Products.Find(orderItem.ProductId);
          orderIt.Items.Add(new OrderItemDTO
            (
              product.Name,
              orderItem.Quantity,
              product.Price
            ));
        }
      }
    }

  }
}
