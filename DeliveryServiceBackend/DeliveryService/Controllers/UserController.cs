using DeliveryService.Data;
using DeliveryService.Model;
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
    private readonly DataContext _context;
    private readonly string _img_repo_base_path= Path.Combine(Directory.GetCurrentDirectory(), "ImgRepo");

    public UserController(DataContext context)
    {
      _context = context;
    }
    
    [HttpPost]
    [Route("api/[controller]/register")]
    public async Task<ActionResult<string>> Post(User newUser)
    {
        string errStr = "";
        if (!newUser.ValidateSelf(out errStr))
          return errStr;

        var result = await _context.Users.FirstOrDefaultAsync(x => x.Email == newUser.Email);
        if ( result != null)
        {
          errStr = "Email already used";
          return BadRequest(errStr);
        }

        result = await  _context.Users.FirstOrDefaultAsync(x => x.Username == newUser.Username);
        if ( result  != null)
        {
          errStr = "Username already used";
          return BadRequest(errStr);
        }

        var file = Request.Form.Files[0];
        if(file !=null && file.Length>0)
        {
          var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
          var fullPath = Path.Combine(_img_repo_base_path, fileName);

          if(System.IO.File.Exists(fullPath))
          {
            fileName = string.Format(@"{0}_usr_img.{1}", Guid.NewGuid(), fileName.Split(new char[] {'.'}.Last(), StringSplitOptions.RemoveEmptyEntries));
            fullPath = Path.Combine(_img_repo_base_path, fileName);
          }

          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            await file.CopyToAsync(stream);
          }
          newUser.ImageName = fileName;
        }

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return Ok("");           
    }

    [HttpPost]
    [Route("api/[controller]/login")]
    public async Task<ActionResult<string>> Login(User newUser)
    {
      return Ok();
    }
  }
}
