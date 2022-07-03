using DeliveryService.Common;
using DeliveryService.Data;
using DeliveryService.DTOs;
using DeliveryService.Model;
using DeliveryService.Services;
using DeliveryService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

namespace DeliveryService.Controllers
{
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly DeliveryDataContext _context;
    private readonly string _img_repo_base_path= Path.Combine(Directory.GetCurrentDirectory(), "ImgRepo");

    private readonly ITransistentUserService _transistentUserService;

    public UserController(DeliveryDataContext context, ITransistentUserService transistentRegisterService)
    {
      _context = context;
      _transistentUserService = transistentRegisterService;
    }

    //User register
    [HttpPost]
    [Route("api/[controller]/register")]
    public async Task<ActionResult<string>> Register([FromBody]UserRegisterRequestDTO regReq)
    {
      string errStr = "";
      if (!_transistentUserService.TryRegister(regReq, out errStr))
        return BadRequest(errStr);
 
        return Ok("");           
    }

    //User login
    [HttpPost]
    [Route("api/[controller]/{username}/login")]
    public async Task<ActionResult<string>> Login(UserLoginRequestDTO loginReq)
    {
      string errMsg = "";
      string token = "";
      if (!_transistentUserService.TryLogin(loginReq, out errMsg, out token)) return BadRequest(errMsg);

      return Ok(token);
    }

    //User profile update
    [HttpPost]
    [Route("api/[controller]/{username}/update")]
    public async Task<ActionResult<string>> Update(UserUpdateRequestDTO loginReq, [System.Web.Http.FromUri] string username)
    {
      string errStr = "";
      if (!_transistentUserService.TryUpdate(loginReq, username, out errStr))
        return BadRequest(errStr);

      return Ok("");
    }

    //Read all users
    [HttpGet]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public async Task<List<UserDTO>> GetAllUsers()
    {
      return _transistentUserService.GetAll();
    }

    //Get personal profile data
    [HttpGet]
    [Route("api/[controller]/{username}/profile")]
    public async Task<UserDTO> Profile([System.Web.Http.FromUri] string username)
    {
      return _transistentUserService.GetByUsername(username);
    }

    //Accept registration
    [HttpPost]
    [Route("api/[controller]/{username}/accept")]
    [Authorize(Roles = "Administrator")]
    public async Task<string> Accept([System.Web.Http.FromUri] string username)
    {
      var errStr = "";
      _transistentUserService.SetState(username, EUserState.Confirmed, out errStr);
      return errStr;
    }

    //Decline registration
    [HttpPost]
    [Route("api/[controller]/{username}/decline")]
    [Authorize(Roles = "Administrator")]
    public async Task<string> Decline([System.Web.Http.FromUri] string username)
    {
      var errStr = "";
      _transistentUserService.SetState(username, EUserState.Rejected, out errStr);
      return errStr;
    }

    //Set registration on pending, wait to be verified
    [HttpPost]
    [Route("api/[controller]/{username}/pending")]
    [Authorize(Roles = "Administrator")]
    public async Task<string> Pending([System.Web.Http.FromUri] string username)
    {
      var errStr = "";
      _transistentUserService.SetState(username, EUserState.Pending, out errStr);
      return errStr;
    }

  }
}
