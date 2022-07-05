using AutoMapper;
using DeliveryService.Common;
using DeliveryService.Data;
using DeliveryService.DTOs;
using DeliveryService.Model;
using DeliveryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DeliveryService.Services.Impl
{
  public class UserService:ITransistentUserService
  {
    private readonly IMapper _mapper;
    private readonly DeliveryDataContext _dbContext;
    private static readonly string _pepper= "dDHsnc1x";
    private static IConfigurationSection _secretKey;

    public UserService(IMapper mapper, IConfiguration config, DeliveryDataContext dbContext)
    {
      this._dbContext = dbContext;
      this._mapper = mapper;
      _secretKey = config.GetSection("SecretKey");
    }

    public bool TryRegister(UserRegisterRequestDTO regReq, out string errMsg)
    {
      errMsg = "";
      if ((errMsg = UserValidator.ValidateUserBaseCriteria(regReq)) !="") return false;
      if ((errMsg = UserValidator.ValidateUserUniqueEmail(regReq, _dbContext)) != "") return false;
      if ((errMsg = UserValidator.ValidateUserUniqueUsername(regReq, _dbContext)) != "") return false;

      string imgName="";
      if (!HandleStoreUserImage(regReq, out imgName)) imgName = User.DEFAULT_IMG_NAME;

      _dbContext.Users.Add
      (
        new User(regReq.Email, ExecuteEncyprion(regReq.Password), regReq.Username, regReq.Name, regReq.Surname,
                 regReq.Birthdate, regReq.Address, MapUserTypeToCode(regReq.Type), imgName)
      );
      _dbContext.SaveChanges();
      return true;
    }

    private string  ExecuteEncyprion(string plainPassword)
    {
      StringBuilder sb = new StringBuilder();
      using (var sha = SHA256.Create())
      {
        Encoding enc = Encoding.UTF8;
        Byte[] result = sha.ComputeHash(enc.GetBytes(GetComparablePasswordPlain(plainPassword)));

        foreach (Byte b in result)
          sb.Append(b.ToString("x2"));
      }
      return sb.ToString();
    }

    public bool TryLogin(UserLoginRequestDTO loginReq, out string errMsg, out string token)
    {
      token = "";
      Dictionary<string, bool> stats = new Dictionary<string, bool>();
      errMsg=UserValidator.ValidateLoginParams(loginReq, out stats);
      if (errMsg == "") return false;

      User usr = _dbContext.Users.FirstOrDefault(x => x.Email.Equals(loginReq.Email));
      if (usr is null) { stats["Email"] = false; errMsg = "Invalid email;"; return false; }

      if (!usr.Password.Equals(ExecuteEncyprion(loginReq.Password)))
      { stats["Password"] = false; errMsg = "Incorrect password;"; return false; }

      List<Claim> claims = new List<Claim>();
      switch( MapUserCodeToType(usr.Type))
      {
        case "administrator": { claims.Add(new Claim(ClaimTypes.Role, "Administrator"));        break; }
        case "deliveryman":   { claims.Add(new Claim(ClaimTypes.Role, "Deliveryman"));          break; }
        case "consumer":      { claims.Add(new Claim(ClaimTypes.Role, "Consumer"));             break; }
      }

      SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
      var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
      var tokeOptions = new JwtSecurityToken(
          issuer: "http://localhost:44398", 
          claims: claims,
          expires: DateTime.Now.AddMinutes(20), 
          signingCredentials: signinCredentials 
      );
      token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

      return true;
    }

    private char MapUserTypeToCode(string type)
    {
      char retVal = 'u';
      switch(type.ToLower())
      {
        case "administrator": { retVal = 'a'; break; }
        case "deliveryman":   { retVal = 'd'; break; }
        case "consumer":      { retVal = 'c'; break; }
      }
      return retVal;
    }

    private string MapUserCodeToType(char code)
    {
      string retVal = "unknown";
      switch (code.ToString().ToLower().First())
      {
        case 'a': { retVal = "administrator"; break; }
        case 'd': { retVal = "deliveryman"; break; }
        case 'c': { retVal = "consumer"; break; }
      }
      return retVal;
    }

    public bool TryUpdate(UserUpdateRequestDTO updateReq, string username, out string errMsg)
    {
      var target = _dbContext.Users.FirstOrDefault(x => x.Username.Equals(username));
      if (target == null)
      { errMsg = "Username does not exists"; return false; }

      if (updateReq.Password != target.Password)
      { errMsg = "Invalid password"; return false; }

      errMsg = "";
      if (updateReq.NewPassword != "" && (errMsg += ";" + UserValidator.ValidatePassword(updateReq.NewPassword)).Equals(String.Empty))
          target.Password = ExecuteEncyprion(updateReq.NewPassword);
     
      Dictionary<string, bool> stats = new Dictionary<string, bool>();
      errMsg += ";" + UserValidator.ValidateUserBaseCriteria(updateReq, out stats);

      foreach (var stat in stats)
      {
        if (stat.Value)
        {
          switch (stat.Key)
          {
            case "Name": { target.Name = updateReq.Name; break; }
            case "Surname": { target.Surname = updateReq.Surname; break; }
            case "Birthdate": { target.Birthdate = updateReq.Birthdate; break; }
            case "Address": { target.Address = updateReq.Address; break; }
            case "ImageName":
              {
                //string imgName = "";
                //if(HandleUpdateUserImage(updateReq, dataContext, out imgName))
                //{
                //  if(!target.ImageName.Equals(User.DEFAULT_IMG_NAME))
                //  {
                //    var basePath = Path.Combine(Directory.GetCurrentDirectory(), "ImgRepo");
                //    var oldImgPath = Path.Combine(basePath, target.ImageName);
                //    if (File.Exists(oldImgPath)) File.Delete(oldImgPath);

                //    var fileName = "";
                //    var newImgPath = Path.Combine(basePath, fileName);
                //    if (File.Exists(newImgPath))
                //    {
                //      fileName = string.Format(@"{0}_usr_img.{1}", Guid.NewGuid(), fileName.Split(new char[] { '.' }.Last(), StringSplitOptions.RemoveEmptyEntries));
                //      newImgPath = Path.Combine(basePath, fileName);
                //    }

                //    using (var stream = new FileStream(newImgPath, FileMode.Create))
                //    {
                //      stream.Write(updateReq.ImageRaw, 0, updateReq.ImageRaw.Length);
                //    }
                //    target.ImageName = fileName;
                //  }

                //}
                break;
              }
          }
        }
      }
      _dbContext.SaveChanges();
      return true;
    }

    public List<UserDTO> GetAll()
    {
      return _mapper.Map<List<UserDTO>>(_dbContext.Users.ToList());
    }

    private bool HandleUpdateUserImage(UserUpdateRequestDTO updateReq, out string imgName)
    {
      imgName = "";

      //TODO:

      return true;
    }

    private bool HandleStoreUserImage(UserRegisterRequestDTO regReq, out string imgName)
    {
      imgName = "";

      //TODO:

      //if (file != null && file.Length > 0)
      //{
      //  var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
      //  var fullPath = Path.Combine(_img_repo_base_path, fileName);

      //  if (System.IO.File.Exists(fullPath))
      //  {
      //    fileName = string.Format(@"{0}_usr_img.{1}", Guid.NewGuid(), fileName.Split(new char[] { '.' }.Last(), StringSplitOptions.RemoveEmptyEntries));
      //    fullPath = Path.Combine(_img_repo_base_path, fileName);
      //  }

      //  using (var stream = new FileStream(fullPath, FileMode.Create))
      //  {
      //    await file.CopyToAsync(stream);
      //  }
      //  newUser.ImageName = fileName;

      return true;
    }

    public UserDTO GetByUsername(string username)
    {
      return _mapper.Map<UserDTO>(_dbContext.Users.Find(username));
    }
    public UserDTO GetByEmail(string email)
    {
      return _mapper.Map<UserDTO>(_dbContext.Users.FirstOrDefault(x=>x.Email.Equals(email)));
    }

    public bool SetState(string username, EUserState state, out string errMsg)
    {
      errMsg = "";
      var user = _dbContext.Users.Find(username);
      if (user == null) { errMsg = "Invalid username"; return false;  }

      user.State = (int)state;
      _dbContext.SaveChanges();
      return true;
    }

    public static string GetComparablePasswordPlain(string rawPassword)
    {
      return rawPassword+_pepper;
    }

  }
}
