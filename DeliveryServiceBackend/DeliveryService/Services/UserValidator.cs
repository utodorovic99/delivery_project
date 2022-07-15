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
    private static readonly string _emailRegexRFC_2822 = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
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

    public static string ValidateUserBaseCriteria(UserRequestDTO req, ref Dictionary<string, bool> stats, List<string> ignoreEmpty)
    {
      if(stats == null) stats = new Dictionary<string, bool>();

      var errMsg = "";
      DateTime dummyResult = new DateTime();
      if ((req.Email == ""   && !ignoreEmpty.Contains("Email"))   ||  (req.Password == ""  && !ignoreEmpty.Contains("Password"))  ||
          (req.Surname == "" && !ignoreEmpty.Contains("Surname")) ||  (req.Address == ""   && !ignoreEmpty.Contains("Address"))   ||
          (req.Type == ""    && !ignoreEmpty.Contains("Type"))    ||  (req.Username == ""  && !ignoreEmpty.Contains("Username"))  ||
          (req.Name == ""    && !ignoreEmpty.Contains("Name"))    ||  (req.Birthdate == "" && !ignoreEmpty.Contains("Birthdate")))
      {
        errMsg += "Empty fields detected.\n";
        if (req.Email     == "" && !ignoreEmpty.Contains("Email"))      stats.Add("Email",     false);  else stats.Add("Email",     true);
        if (req.Password  == "" && !ignoreEmpty.Contains("Password"))   stats.Add("Password", false);  else stats.Add("Password",  true);
        if (req.Surname   == "" && !ignoreEmpty.Contains("Surname"))    stats.Add("Surname",   false);  else stats.Add("Surname",   true);
        if (req.Address   == "" && !ignoreEmpty.Contains("Address"))    stats.Add("Address",   false);  else stats.Add("Address",   true);
        if (req.Type      == "" && !ignoreEmpty.Contains("Type"))       stats.Add("Type",      false);  else stats.Add("Type",      true);
        if (req.Username  == "" && !ignoreEmpty.Contains("Username"))   stats.Add("Username",  false);  else stats.Add("Username",  true);
        if (req.Name      == "" && !ignoreEmpty.Contains("Name"))       stats.Add("Name",      false);  else stats.Add("Name",      true);
        if (req.Birthdate == "" && !ignoreEmpty.Contains("Birthdate"))  stats.Add("Birthdate", false);  else stats.Add("Birthdate", true);
      }
      else
      {
        if (req.Birthdate!="" && !DateTime.TryParseExact(req.Birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dummyResult))
          { errMsg += "Bad date format (must be YYYY-MM-DD).\n";           stats["Birthdate"] = false; }

        if (req.Password!= "" && req.Password.Length < 8)
          { errMsg += "Password must not be shorter than 8 characters\n";  stats["Password"] = false; }

        if (req.Username != "" && req.Username.Length < 8)
          { errMsg +=  "Username must not be shorter than 8 characters\n"; stats["Username"] = false; }

        if (req.Address != "" && req.Address.Length < 8)
          { errMsg += "Address must not be shorter than 8 characters\n";   stats["Address"] = false; };

        if (!(new Regex(_emailRegexRFC_2822)).IsMatch(req.Email))
          { errMsg += "Invalid Email format (examle: somename@somedomain.com)\n"; stats["Email"] = false; }
      }

      errMsg.TrimEnd('\n');
      return errMsg;
    }

    public static string ValidateUserBaseCriteria(UserUpdateRequestDTO req, ref Dictionary<string, bool> stats, List<string> ignoreEmpty)
    {
      if (stats == null) stats = new Dictionary<string, bool>();

      var errMsg = "";
      DateTime dummyResult = new DateTime();
      if ((req.Email == "" && !ignoreEmpty.Contains("Email")) || (req.Password == "" && !ignoreEmpty.Contains("Password")) ||
          (req.Surname == "" && !ignoreEmpty.Contains("Surname")) || (req.Address == "" && !ignoreEmpty.Contains("Address")) ||
          (req.Type == "" && !ignoreEmpty.Contains("Type")) || (req.Username == "" && !ignoreEmpty.Contains("Username")) ||
          (req.Name == "" && !ignoreEmpty.Contains("Name")) || (req.Birthdate == "" && !ignoreEmpty.Contains("Birthdate")))
      {
        errMsg += "Empty fields detected.\n";
        if (req.Email == "" && !ignoreEmpty.Contains("Email")) stats.Add("Email", false); else stats.Add("Email", true);
        if (req.Password == "" && !ignoreEmpty.Contains("Password")) stats.Add("Password", false); else stats.Add("Password", true);
        if (req.Surname == "" && !ignoreEmpty.Contains("Surname")) stats.Add("Surname", false); else stats.Add("Surname", true);
        if (req.Address == "" && !ignoreEmpty.Contains("Address")) stats.Add("Address", false); else stats.Add("Address", true);
        if (req.Type == "" && !ignoreEmpty.Contains("Type")) stats.Add("Type", false); else stats.Add("Type", true);
        if (req.Username == "" && !ignoreEmpty.Contains("Username")) stats.Add("Username", false); else stats.Add("Username", true);
        if (req.Name == "" && !ignoreEmpty.Contains("Name")) stats.Add("Name", false); else stats.Add("Name", true);
        if (req.Birthdate == "" && !ignoreEmpty.Contains("Birthdate")) stats.Add("Birthdate", false); else stats.Add("Birthdate", true);
      }
      else
      {
        stats.Add("Email", true);
        stats.Add("Password", true);
        stats.Add("Surname", true);
        stats.Add("Address", true);
        stats.Add("Type", true);
        stats.Add("Username", true);
        stats.Add("Name", true);
        stats.Add("Birthdate", true);
         
        if (req.Birthdate != "" && !DateTime.TryParseExact(req.Birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dummyResult))
        { errMsg += "Bad date format (must be YYYY-MM-DD).\n"; stats["Birthdate"] = false; }

        if (req.Password != "" && req.Password.Length < 8)
        { errMsg += "Password must not be shorter than 8 characters\n"; stats["Password"] = false; }

        if (req.Username != "" && req.Username.Length < 8)
        { errMsg += "Username must not be shorter than 8 characters\n"; stats["Username"] = false; }

        if (req.Address != "" && req.Address.Length < 8)
        { errMsg += "Address must not be shorter than 8 characters\n"; stats["Address"] = false; };

        if (!(new Regex(_emailRegexRFC_2822)).IsMatch(req.Email))
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
      if (!((new Regex(_emailRegexRFC_2822)).IsMatch(email)))
        errMsg += "Invalid Email format (examle: somename@somedomain.com)\n";
      return errMsg;
    }

    public static string ValidateLoginParams(UserLoginRequestDTO loginReq, ref Dictionary<string, bool> stats)
    {
      string errMsg = "";
      string tmpStr = "";
      if(stats == null) stats = new Dictionary<string, bool>();
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
