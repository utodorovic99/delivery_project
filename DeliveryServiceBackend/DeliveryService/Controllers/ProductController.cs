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
    public async Task<string> Order([System.Web.Http.FromUri] string username, [FromBody] OrderDTO orderDTO)
    {
      throw new NotImplementedException();
    }

    //Consumer confirms order (it is delivered)
    [HttpPost]
    [Route("api/[controller]/orders/{orderId}/confirm")]
    [Authorize(Roles = "Consumer")]
    public async Task<string> ConfirmDelivery([System.Web.Http.FromUri] int orderId)
    {
      throw new NotImplementedException();
    }

    //All orders (for Administrator)
    [HttpGet]
    [Route("api/[controller]/orders")]
    [Authorize(Roles = "Administrator")]
    public async Task<List<OrderDTO>> Orders()
    {
      throw new NotImplementedException();
    }

    //Get history of my orders (for Consumer)
    [HttpGet]
    [Route("api/[controller]/orders/confirmed-for/{username}")]
    [Authorize(Roles = "Consumer")]
    public async Task<OrderDTO> ConfirmedOrdersFor([System.Web.Http.FromUri] string username)
    {
      throw new NotImplementedException();
    }

    //Get history of my completed orders (for Deliveryman)
    [HttpGet]
    [Route("api/[controller]/orders/completed-for/{username}")]
    [Authorize(Roles = "Deliveryman")]
    public async Task<OrderDTO> CompletedOrdersFor([System.Web.Http.FromUri] string username)
    {
      throw new NotImplementedException();
    }

    //Get all available orders (not taken yet - for Deliveryman)
    [HttpGet]
    [Route("api/[controller]/orders/available-for/{username}")]
    [Authorize(Roles = "Deliveryman")]
    public async Task<string> AvailableOrdersFor([System.Web.Http.FromUri] string username)
    {
      throw new NotImplementedException();
    }

    //Take order for delivery (for Deliveryman)
    [HttpPost]
    [Route("api/[controller]/orders/accept/orderId")]
    [Authorize(Roles = "Deliveryman")]
    public async Task<string> AcceptOrder([System.Web.Http.FromUri] int orderId, [FromBody] string username)
    {
      throw new NotImplementedException();
    }

    //Create new product (for Administrator)
    [HttpPost]
    [Route("api/[controller]/products/create")]
    [Authorize(Roles = "Administrator")]
    public async Task<string> CreateProduct([FromBody] ProductDTO product)
    {
      throw new NotImplementedException();
    }
  }
}
