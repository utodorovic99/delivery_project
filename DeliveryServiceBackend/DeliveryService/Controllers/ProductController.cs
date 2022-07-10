using DeliveryService.Data;
using DeliveryService.DTOs;
using DeliveryService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryService.Controllers
{
  [ApiController]
  public class ProductController : ControllerBase
  {

    private readonly DeliveryDataContext _context;
    private readonly string _img_repo_base_path = Path.Combine(Directory.GetCurrentDirectory(), "ImgRepo");

    private readonly ITransistentProductService _transistentProductsService;
    private readonly ITransistentUserService _transistentUserService;

    public ProductController(DeliveryDataContext context, ITransistentProductService _transistentProductsService, ITransistentUserService _transistentUserService)
    {
      _context = context;
      this._transistentProductsService  = _transistentProductsService;
      this._transistentUserService      = _transistentUserService;
    }

    //Consumer executes order
    [HttpPost]
    [Route("api/[controller]/orders/{username}")]
    [Authorize(Roles = "Consumer")]
    public ActionResult<PrimitiveResponseDTO> Order([System.Web.Http.FromUri] string username, [FromBody] OrderDTO orderDTO)
    {
      var errMsg = "";
      if (_transistentProductsService.PublishOrder(orderDTO, out errMsg))
        return Ok(new PrimitiveResponseDTO("","string"));
      else
        return BadRequest(errMsg);
    }

    //Consumer confirms order (it is delivered)
    [HttpPost]
    [Route("api/[controller]/orders/{orderId}/confirm")]
    [Authorize(Roles = "Consumer")]
    public ActionResult<PrimitiveResponseDTO> ConfirmDelivery([System.Web.Http.FromUri] int orderId)
    {
      var errMsg = "";
      if (_transistentProductsService.ConfirmDelivery(orderId, out errMsg))
        return Ok(new PrimitiveResponseDTO("", "string"));
      else
        return BadRequest(errMsg);
    }

    //All orders (for Administrator)
    [HttpGet]
    [Route("api/[controller]/orders")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<List<OrderDTO>> Orders()
    {
      return Ok(_transistentProductsService.GetAllOrders());
    }

    //All orders items for selected order
    [HttpGet]
    [Route("api/[controller]/orders/{orderId}/items")]
    public ActionResult<List<OrderItemDTO>> OrderItems([System.Web.Http.FromUri] int orderId)
    {
      
      var errMsg = "";
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      string username = "";
      if (identity != null)
        username = identity.FindFirst("username").Value;

      //Can see if i'm admin, if i'm one who ordered it, i'm one who delivers it or if it is still has not deliveryman
      var role = _transistentUserService.GetRole(username);
      if (role == "a" ||
          _transistentProductsService.IsOrderAvailable(orderId, out errMsg) ||
         (role == "c" && _transistentProductsService.IsOrderConsumer(orderId, username)) ||
         (role == "d" && _transistentProductsService.IsOrderDeliveryman(orderId, username)))
      {

        var outVal = _transistentProductsService.GetOrderItems(orderId, out errMsg);
        if (errMsg == "")
          return Ok(outVal);
        else
          return BadRequest(errMsg);
      }
      return BadRequest("Access denied");
    }

    //Get history of my orders (for Consumer)
    [HttpGet]
    [Route("api/[controller]/orders/confirmed-for/{username}")]
    [Authorize(Roles = "Consumer")]
    public ActionResult<List<OrderDTO>> ConfirmedOrdersFor([System.Web.Http.FromUri] string username)
    {
      var errMsg = "";
      var result = _transistentProductsService.ConfirmedOrdersFor(username, out errMsg);
      if (errMsg == "") return result;
      return BadRequest(errMsg);
    }

    //Get history of my completed orders (for Deliveryman)
    [HttpGet]
    [Route("api/[controller]/orders/completed-for/{username}")]
    [Authorize(Roles = "Deliveryman")]
    public ActionResult<List<OrderDTO>> CompletedOrdersFor([System.Web.Http.FromUri] string username)
    {
      var errMsg = "";
      var result = _transistentProductsService.CompletedOrdersFor(username, out errMsg);
      if (errMsg == "") return result;
      return BadRequest(errMsg);
    }


    //Get my current taken delivery (for Deliveryman)
    [HttpGet]
    [Route("api/[controller]/orders/current-for/{username}")]
    [Authorize(Roles = "Deliveryman,Consumer")]
    public ActionResult<List<OrderDTO>>CurrentOrderFor([System.Web.Http.FromUri] string username)
    {
      var errMsg = "";
      var userRole = _transistentUserService.GetRole(username);
      var result = _transistentProductsService.CurrentOrderFor(username, userRole, out errMsg);
      if (errMsg == "") return result;
      return BadRequest(errMsg);
    }

    //Get all available orders (not taken yet - for Deliveryman)
    [HttpGet]
    [Route("api/[controller]/orders/available-for/{username}")]
    [Authorize(Roles = "Deliveryman")]
    public ActionResult<List<OrderDTO>> AvailableOrdersFor([System.Web.Http.FromUri] string username)
    {
      var errMsg = "";
      var result = _transistentProductsService.AvailableOrdersFor(username, out errMsg);
      if (errMsg == "") return result;
      return BadRequest(errMsg);
    }

    //Take order for delivery (for Deliveryman)
    [HttpPost]
    [Route("api/[controller]/orders/accept/{orderId}")]
    [Authorize(Roles = "Deliveryman")]
    public ActionResult<PrimitiveResponseDTO> AcceptOrder([System.Web.Http.FromUri] int orderId, [FromBody] string username)
    {
      var errMsg = "";
      var result = _transistentProductsService.AcceptOrder(orderId, username ,out errMsg);
      if (errMsg == "") return new PrimitiveResponseDTO(result,"string");
      return BadRequest(errMsg);
    }

    //Create new product (for Administrator)
    [HttpPost]
    [Route("api/[controller]/products/create")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<PrimitiveResponseDTO> CreateProduct([FromBody] ProductDTO product)
    {
      var errMsg = "";
      var result = _transistentProductsService.CreateProduct(product, out errMsg);
      if (errMsg == "") return new PrimitiveResponseDTO("", "string");
      return BadRequest(errMsg);
    }

    //Get all products
    [HttpGet]
    [Route("api/[controller]/products")]
    public ActionResult<List<ProductDTO>>Products()
    {
      var errMsg = "";
      var result = _transistentProductsService.GetAllProducts(out errMsg);
      if (errMsg == "") return result;
      return BadRequest(errMsg);
    }

    //Get all product ingredients
    [HttpGet]
    [Route("api/[controller]/products/ingredients")]
    public ActionResult<List<string>> ProductIngredients()
    {
      var errMsg = "";
      var result = _transistentProductsService.GetAllProductIngredients(out errMsg);
      if (errMsg == "") return Ok(result);
      return BadRequest(errMsg);
    }
  }
}
