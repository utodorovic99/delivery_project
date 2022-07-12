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
using System.Net.Http.Headers;
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
    private readonly string _img_repo_base_path = Path.Combine(Directory.GetCurrentDirectory(), "ImgRepo");
    private static readonly int _MAX_IMG_SIZE_B = 500000;

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
      if (regReq.ImageRaw.Length > _MAX_IMG_SIZE_B)
        { errMsg = "Image is too large (max. 500kB)"; return false; }

      string imgName="";
      if (regReq.ImageRaw.Length > 0 && !HandleStoreUserImage(regReq, out imgName)) imgName = "";

      var type = MapUserTypeToCode(regReq.Type);
      var newUser = new User(regReq.Email, ExecuteEncyprion(regReq.Password), regReq.Username, regReq.Name, regReq.Surname,
                 regReq.Birthdate, regReq.Address, type, imgName);

      if (type == 'c') newUser.State = (int)EUserState.Confirmed;

      _dbContext.Users.Add( newUser);
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
      errMsg=UserValidator.ValidateLoginParams(loginReq, ref stats);
      if (errMsg == "") return false;

      User usr = _dbContext.Users.FirstOrDefault(x => x.Email.Equals(loginReq.Email));
      if (usr is null) { stats["Email"] = false; errMsg = "Invalid email;"; return false; }

      if (!usr.Password.Equals(ExecuteEncyprion(loginReq.Password)))
      { stats["Password"] = false; errMsg = "Incorrect password;"; return false; }

      List<Claim> claims = new List<Claim>();
      switch( MapUserCodeToType(usr.Type))
      {
        case "admin":     { claims.Add(new Claim(ClaimTypes.Role, "Administrator"));        break; }
        case "delivery":  { claims.Add(new Claim(ClaimTypes.Role, "Deliveryman"));          break; }
        case "consumer":  { claims.Add(new Claim(ClaimTypes.Role, "Consumer"));             break; }
      }
      claims.Add(new Claim("username", usr.Username));

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
        case "admin":     { retVal = 'a'; break; }
        case "delivery":  { retVal = 'd'; break; }
        case "consumer":  { retVal = 'c'; break; }
      }
      return retVal;
    }

    private string MapUserCodeToType(char code)
    {
      string retVal = "unknown";
      switch (code.ToString().ToLower().First())
      {
        case 'a': { retVal = "admin"; break; }
        case 'd': { retVal = "delivery"; break; }
        case 'c': { retVal = "consumer"; break; }
      }
      return retVal;
    }

    public bool TryUpdate(UserUpdateRequestDTO updateReq, string username, out string errMsg)
    {
      var target = _dbContext.Users.FirstOrDefault(x => x.Username.Equals(username));
      if (target == null)
      { errMsg = "Username does not exists"; return false; }

      if (ExecuteEncyprion(updateReq.Password) != target.Password)
      { errMsg = "Invalid password"; return false; }

      errMsg = "";
      Dictionary<string, bool> stats = new Dictionary<string, bool>();
      stats.Add("NewPassword", true); 
      if (!(updateReq.NewPassword != "" && (errMsg= UserValidator.ValidatePassword(updateReq.NewPassword) + ";").Equals(";")))           
      {
        errMsg = "New " + errMsg.ToLower();
        stats["NewPassword"] = false;
      }
     
      errMsg += ";" + UserValidator.ValidateUserBaseCriteria(updateReq, ref stats, new List<string>() {"Username", "Type"});

      if (_dbContext.Users.FirstOrDefault(x => x.Email.Equals(updateReq.Email) && !x.Username.Equals(target.Username)) != null)
      {
        errMsg += ";" + "Email already used by another account";
        if (!stats.ContainsKey("Email")) stats.Add("Email", false);
      }

      bool partialPassed = false;
      foreach (var stat in stats)
      {
        if (stat.Value)
        {
          switch (stat.Key)
          {
            case "Name":        { target.Name = updateReq.Name;                              partialPassed = true; break; }
            case "Surname":     { target.Surname = updateReq.Surname;                        partialPassed = true; break; }
            case "Birthdate":   { target.Birthdate = updateReq.Birthdate;                    partialPassed = true; break; }
            case "Address":     { target.Address = updateReq.Address;                        partialPassed = true; break; }
            case "Email":       { target.Email = updateReq.Email;                            partialPassed = true; break; }
            case "NewPassword": { target.Password = ExecuteEncyprion(updateReq.NewPassword); partialPassed = true; break; }
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
      errMsg = errMsg.TrimStart(new char[] { ';' });
      return partialPassed;
    }

    public List<UserDTO> GetAll()
    {
      var users= _mapper.Map<List<UserDTO>>(_dbContext.Users.ToList());
      foreach (var user in users)
        user.ImageRaw = LoadUserImage(user);    

      return users;
    }

    private byte[] LoadUserImage(UserDTO user)
    {
      string imgName = "";
      var fullPath = "";
      imgName = _dbContext.Users.Find(user.Username).ImageName;
      fullPath = Path.Combine(_img_repo_base_path, imgName);
      if (System.IO.File.Exists(fullPath))
      {
         return System.IO.File.ReadAllBytes(fullPath);
      }
      return new byte[0];
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

      if (regReq.ImageRaw != null && regReq.ImageRaw.Length > 0)
      {
        var fileName = ContentDispositionHeaderValue.Parse(regReq.ImageRaw.ContentDisposition).FileName.Trim('"');
        var fullPath = Path.Combine(_img_repo_base_path, fileName);

        if (System.IO.File.Exists(fullPath))
        {
          var fileNameParts = fileName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
          if (fileNameParts.First().Length > 15) fileNameParts[0] = "";

          fileName = string.Format(@"{0}_{1}.{2}", Guid.NewGuid(), fileNameParts[0], fileNameParts.Last());
          fullPath = Path.Combine(_img_repo_base_path, fileName);
        }

        Task.Factory.StartNew(() =>
        {
          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            regReq.ImageRaw.CopyTo(stream);
          }
        });

        imgName = fileName;
        return true;
      }
      return false;
    }

    public UserDTO GetByUsername(string username)
    {
      var user=_mapper.Map<UserDTO>(_dbContext.Users.Find(username));
      user.ImageRaw = LoadUserImage(user);
      return user;
    }
    public UserDTO GetByEmail(string email)
    {
      var user =_mapper.Map<UserDTO>(_dbContext.Users.FirstOrDefault(x=>x.Email.Equals(email)));
      user.ImageRaw = LoadUserImage(user);
      return user;
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

    public bool ValidatePassword(string username, string passwordPlain)
    {
      try
      {
        return ExecuteEncyprion(passwordPlain).Equals(_dbContext.Users.FirstOrDefault(x => x.Username.Equals(username)).Password);
      }
      catch (Exception exc)
      { return false; }
    }

    public string GetRole(string username)
    {
      return _dbContext.Users.ToList().Where(x => x.Username.Equals(username)).ToList().First().Type.ToString();
    }

  }
}
