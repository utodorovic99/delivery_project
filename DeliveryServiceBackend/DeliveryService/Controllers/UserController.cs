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
using Newtonsoft.Json;
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
    public ActionResult<string> Register([FromBody]UserRegisterRequestDTO regReq)
    {
      string errStr = "";
      if (!_transistentUserService.TryRegister(regReq, out errStr))
        return BadRequest(errStr);
 
        return Ok("");           
    }

    //User login
    [HttpPost]
    [Route("api/[controller]/login")]
    public ActionResult<PrimitiveResponseDTO> Login([FromBody]UserLoginRequestDTO loginReq)
    {
      string errMsg = "";
      string token = "";
      if (!_transistentUserService.TryLogin(loginReq, out errMsg, out token)) return BadRequest(errMsg);

      return Ok(new PrimitiveResponseDTO(token, "String"));
    }

    //User profile update
    [HttpPost]
    [Route("api/[controller]/{username}/update")]
    public  ActionResult<PrimitiveResponseDTO> Update([FromBody]UserUpdateRequestDTO updateReq, [System.Web.Http.FromUri] string username)
    {
      string errStr = "";
      bool isPasswordValid = _transistentUserService.ValidatePassword(username, updateReq.Password);
      if(!isPasswordValid) return BadRequest("Invalid password");

      if (!_transistentUserService.TryUpdate(updateReq, username, out errStr))
        return BadRequest(errStr);

      string passStatus = "";
      if (isPasswordValid && _transistentUserService.ValidatePassword(username, updateReq.NewPassword))
        passStatus = "T";
      else
        passStatus = "F";

      return Ok(new  PrimitiveResponseDTO(errStr+passStatus, "string"));
    }

    //Read all users
    [HttpGet]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<List<UserDTO>> GetAllUsers()
    {
      return Ok(_transistentUserService.GetAll());
    }

    //Get personal profile data
    [HttpGet]
    [Route("api/[controller]/{username}/profile")]
    public ActionResult<UserDTO> Profile([System.Web.Http.FromUri] string username)
    {
      return Ok(_transistentUserService.GetByUsername(username));
    }

    //Accept registration
    [HttpPost]
    [Route("api/[controller]/{username}/accept")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<PrimitiveResponseDTO> Accept([System.Web.Http.FromUri] string username)
    {
      var errStr = "";
      _transistentUserService.SetState(username, EUserState.Confirmed, out errStr);
      if (errStr != "") return BadRequest(new PrimitiveResponseDTO(errStr, "string"));

      return Ok(new PrimitiveResponseDTO("", "string"));
    }

    //Decline registration
    [HttpPost]
    [Route("api/[controller]/{username}/decline")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<PrimitiveResponseDTO> Decline([System.Web.Http.FromUri] string username)
    {
      var errStr = "";
      _transistentUserService.SetState(username, EUserState.Rejected, out errStr);
      if (errStr != "") return BadRequest(new PrimitiveResponseDTO(errStr, "string"));

      return Ok(new PrimitiveResponseDTO("", "string"));
    }

    //Set registration on pending, wait to be verified
    [HttpPost]
    [Route("api/[controller]/{username}/pending")]
    [Authorize(Roles = "Administrator")]
    public ActionResult<PrimitiveResponseDTO> Pending([System.Web.Http.FromUri] string username)
    {
      var errStr = "";
      _transistentUserService.SetState(username, EUserState.Pending, out errStr);
      if (errStr != "") return BadRequest(new PrimitiveResponseDTO(errStr, "string"));

      return Ok(new PrimitiveResponseDTO("", "string"));
    }

  }
}
