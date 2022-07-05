using DeliveryService.Data;
using DeliveryService.DTOs;
using DeliveryService.Model;
using DeliveryService.Services.Impl;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DeliveryService.Services
{
  public class UserValidator
  {
    public static string ValidateUserBaseCriteria(UserRequestDTO req)
    {
      var errMsg = "";
      DateTime dummyResult = new DateTime();
      if (req.Email == "" || req.Password == "" || req.Surname == "" || req.Address == "" || req.Type == "" || req.Username == "" || req.Name == "" || req.Birthdate == "")
        errMsg += "Empty fields detected.\n";
      else
      {
        if (!DateTime.TryParseExact(req.Birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dummyResult))
          errMsg += "Bad date format (must be YYYY-MM-DD).\n";

        if (req.Password.Length < 8) errMsg += "Password must not be shorter than 8 characters\n";

        if (req.Username.Length < 8) errMsg += "Username must not be shorter than 8 characters\n";

        if (req.Address.Length < 8) errMsg += "Address must not be shorter than 8 characters\n";

        if (!((new Regex(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$")).IsMatch(req.Email)))
          errMsg += "Invalid Email format (examle: somename@somedomain.com)\n";
      }

      errMsg.TrimEnd('\n');
      return errMsg;
    }

    public static string ValidateUserBaseCriteria(UserRequestDTO req, out Dictionary<string, bool> stats)
    {
      stats = new Dictionary<string, bool>();
      var errMsg = "";
      DateTime dummyResult = new DateTime();
      if (req.Email == "" || req.Password == "" || req.Surname == "" || req.Address == "" || req.Type == "" || req.Username == "" || req.Name == "" || req.Birthdate == "")
      {
        errMsg += "Empty fields detected.\n";
        if (req.Email     == "")  stats.Add("Email",     false);  else stats.Add("Email",     true);
        if (req.Password  == "")  stats.Add("Password",  false);  else stats.Add("Password",  true);
        if (req.Surname   == "")  stats.Add("Surname",   false);  else stats.Add("Surname",   true);
        if (req.Address   == "")  stats.Add("Address",   false);  else stats.Add("Address",   true);
        if (req.Type      == "")  stats.Add("Type",      false);  else stats.Add("Type",      true);
        if (req.Username  == "")  stats.Add("Username",  false);  else stats.Add("Username",  true);
        if (req.Name      == "")  stats.Add("Name",      false);  else stats.Add("Name",      true);
        if (req.Birthdate == "")  stats.Add("Birthdate", false);  else stats.Add("Birthdate", true);
      }
      else
      {
        if (!DateTime.TryParseExact(req.Birthdate, "YYYY-MM-DD", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dummyResult))
          { errMsg += "Bad date format (must be YYYY-MM-DD).\n";           stats["Birthdate"] = false; }

        if (req.Password.Length < 8)
          { errMsg += "Password must not be shorter than 8 characters\n";  stats["Password"] = false; }

        if (req.Username.Length < 8)
          { errMsg +=  "Username must not be shorter than 8 characters\n"; stats["Username"] = false; }

        if (req.Address.Length < 8)
          { errMsg += "Address must not be shorter than 8 characters\n";   stats["Address"] = false; };

        if (!(new Regex(@"/^[a-zA-Z0-9.! #$%&'*+/=? ^_`{|}~-]+@[a-zA-Z0-9-]+(?:\. [a-zA-Z0-9-]+)*$/")).IsMatch(req.Email))
          { errMsg += "Invalid Email format (examle: somename@somedomain.com)\n"; stats["Email"] = false; }
      }

      errMsg.TrimEnd('\n');
      return errMsg;
    }

    public static string ValidateUserUniqueEmail(UserRequestDTO req, DeliveryDataContext dataContext)
    {
      var errMsg = "";
      var result = dataContext.Users.FirstOrDefault(x => x.Email == req.Email);
      if (result != null)
      {
        errMsg = "Email already used";
      }
      return errMsg;
    }

    public static string ValidateUserUniqueUsername(UserRequestDTO req, DeliveryDataContext dataContext)
    {
      var errMsg = "";
      var result = dataContext.Users.FirstOrDefault(x => x.Username == req.Username);
      if (result != null)
        errMsg = "Email already used";

      return errMsg;
    }

    public static string ValidatePassword(string password)
    {
      var errMsg = "";
      if (password.Length < 8) errMsg += "Password must not be shorter than 8 characters\n";
      return errMsg;
    }

    public static string ValidateUsername(string username)
    {
      return username.Length >= 8 ? "" : "Username must not be shorter than 8 characters\n";
    }

    public static string ValidateEmail(string email)
    {
      var errMsg = "";
      if (!((new Regex(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$")).IsMatch(email)))
        errMsg += "Invalid Email format (examle: somename@somedomain.com)\n";
      return errMsg;
    }

    public static string ValidateLoginParams(UserLoginRequestDTO loginReq, out Dictionary<string, bool> stats)
    {
      string errMsg = "";
      string tmpStr = "";
      stats = new Dictionary<string, bool>();
      stats.Add("Username", true);
      stats.Add("Password", true);

      tmpStr = ValidateEmail(loginReq.Email);
      if (tmpStr != "") { stats["Email"] = false; }
      errMsg += (tmpStr.TrimEnd('\n')+";");

      tmpStr = ValidatePassword(loginReq.Password);
      if (tmpStr != "") { stats["Password"] = false;}
      errMsg += (tmpStr.TrimEnd('\n') + ";");
      if (errMsg != "") return errMsg;
      return errMsg;
    }
  }
}
