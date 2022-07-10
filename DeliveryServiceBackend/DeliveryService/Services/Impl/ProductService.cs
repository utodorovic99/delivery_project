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

    public bool ConfirmDelivery(int orderId, out string errMsg)
    {
      errMsg = "";

      var order = _dbContext.Orders.Find(orderId);                        //Find order
      if (order == null)
      { errMsg = "Invalid order number."; return false; }

      var deliveryman = _dbContext.Users.Find(order.Deliveryman);         //Get deliveryman who accepted it
      if (deliveryman == null)
      { errMsg = "Invalid useranme."; return false; }

      if (!deliveryman.Type.Equals("d"))
      { errMsg = "Invalid user type."; return false; }

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

    public List<OrderDTO> GetAllOrders()
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
      Product product = null;
      Dictionary<int, List<OrderItem>> orderItems = new Dictionary<int, List<OrderItem>>(); //To avoid multiple streams opened at the momement issue
      foreach (var order in orders)
        orderItems.Add(order.Id, new List<OrderItem>());

      foreach (var orderItem in _dbContext.OrderItems)  //Iterate trough order items
        orderItems[orderItem.OrderId].Add(orderItem);   //Attach to order

      foreach(var order in orders)
      {
        foreach(var orderItem in orderItems[order.Id])
        {
          product = _dbContext.Products.Find(orderItem.ProductId);
          order.Items.Add(new OrderItemDTO(product.Name, orderItem.Quantity, product.Price));
        }
      }
    }

    public List<OrderItemDTO> GetOrderItems(int orderId, out string errMsg)
    {
      errMsg = "";
      if (_dbContext.Orders.Find(orderId) == null)
      { errMsg = "Invalid order Id"; return new List<OrderItemDTO>(); }

      var outVal = new List<OrderItemDTO>();
      var items = _dbContext.OrderItems.ToList().Where(x => x.OrderId.Equals(orderId));
      Product product = null;
      foreach (var item in items)
      {
        product = _dbContext.Products.Find(item.ProductId);
        outVal.Add(new OrderItemDTO(product.Name, item.Quantity, product.Price));
      }
      return outVal;
    }

    public List<ProductDTO> GetAllProducts(out string errMsg)
    {
      errMsg = "";
      List<ProductDTO> products = new List<ProductDTO>();
      AttachIngredientsToProducts(ref products);
      return products;
    }

    private void AttachIngredientsToProducts(ref List<ProductDTO> products)
    {
      
      var productsFull = _dbContext.Products.ToList();                       //With ID
      products = _mapper.Map<List<ProductDTO>>(_dbContext.Products.ToList());//WithoutID

      productsFull.OrderBy(x => x.Name);  //Align
      products.OrderBy(x => x.Name);

      List<Ingredient> ingredients = _dbContext.Ingredients.ToList(); //Get all ingredients (few of them)
      List<ProductDefiniton> definitions = null;
      for(int prodloc=0; prodloc<products.Count();++ prodloc)
      {
        definitions = _dbContext.ProductDefinitions.Where(x => x.ProductId.Equals(productsFull[prodloc].Id)).ToList();  //Get all definitions for curr. prod.
        foreach (var definition in definitions)                                                                         //Get ingredient name for each
          products[prodloc].Ingredients.Add(ingredients.FirstOrDefault(x=>x.Id.Equals(definition.IngredientId)).Name);  //Attach
      }

    }

    public List<string> GetAllProductIngredients(out string errMsg)
    {
      errMsg = "";
      return _dbContext.Ingredients.Select(x => x.Name).ToList();
    }
  }
}
