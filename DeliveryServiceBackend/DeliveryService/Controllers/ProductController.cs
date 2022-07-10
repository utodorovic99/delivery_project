using DeliveryService.Data;
using DeliveryService.DTOs;
using DeliveryService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers
{
  [ApiController]
  public class ProductController : ControllerBase
  {

    private readonly DeliveryDataContext _context;
    private readonly string _img_repo_base_path = Path.Combine(Directory.GetCurrentDirectory(), "ImgRepo");

    private readonly ITransistentProductService _transistentRegisterService;

    public ProductController(DeliveryDataContext context, ITransistentProductService transistentRegisterService)
    {
      _context = context;
      _transistentRegisterService = transistentRegisterService;
    }

    //Consumer executes order
    [HttpPost]
    [Route("api/[controller]/orders/{username}")]
    [Authorize(Roles = "Consumer")]
    public ActionResult<PrimitiveResponseDTO> Order([System.Web.Http.FromUri] string username, [FromBody] OrderDTO orderDTO)
    {
      var errMsg = "";
      if (_transistentRegisterService.PublishOrder(orderDTO, out errMsg))
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
      if (_transistentRegisterService.ConfirmDelivery(orderId, out errMsg))
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
      return Ok(_transistentRegisterService.GetAllOrders());
    }

    //All orders items for selected order (for Administrator)
    [HttpGet]
    [Route("api/[controller]/orders/{orderId}/items")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<List<OrderItemDTO>> OrderItems([System.Web.Http.FromUri] int orderId)
    {
      var errMsg = "";
      var outVal = _transistentRegisterService.GetOrderItems(orderId, out errMsg);
      if (errMsg=="")
        return Ok(outVal);
      else
        return BadRequest(errMsg);
    }

    //Get history of my orders (for Consumer)
    [HttpGet]
    [Route("api/[controller]/orders/confirmed-for/{username}")]
    [Authorize(Roles = "Consumer")]
    public ActionResult<List<OrderDTO>> ConfirmedOrdersFor([System.Web.Http.FromUri] string username)
    {
      var errMsg = "";
      var result = _transistentRegisterService.ConfirmedOrdersFor(username, out errMsg);
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
      var result = _transistentRegisterService.CompletedOrdersFor(username, out errMsg);
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
      var result = _transistentRegisterService.AvailableOrdersFor(username, out errMsg);
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
      var result = _transistentRegisterService.AcceptOrder(orderId, username ,out errMsg);
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
      var result = _transistentRegisterService.CreateProduct(product, out errMsg);
      if (errMsg == "") return new PrimitiveResponseDTO("", "string");
      return BadRequest(errMsg);
    }

    //Get all products
    [HttpGet]
    [Route("api/[controller]/products")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<List<ProductDTO>>Products()
    {
      var errMsg = "";
      var result = _transistentRegisterService.GetAllProducts(out errMsg);
      if (errMsg == "") return result;
      return BadRequest(errMsg);
    }

    //Get all product ingredients
    [HttpGet]
    [Route("api/[controller]/products/ingredients")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<List<string>> ProductIngredients()
    {
      var errMsg = "";
      var result = _transistentRegisterService.GetAllProductIngredients(out errMsg);
      if (errMsg == "") return Ok(result);
      return BadRequest(errMsg);
    }
  }
}
