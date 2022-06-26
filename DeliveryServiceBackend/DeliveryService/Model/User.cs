using DeliveryService.Common;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DeliveryService.Model
{
  public class User
  {

    private static readonly string _DEFAULT_IMG_NAME= @"select_image.png";
    private string _birthDate = "";
    private string _imgName = "";

    public User(string email, string password, string username, string name, string surname, string birthdate, string address, string type, string imageName)
    {
      Email = email;
      Password = password;
      Username = username;
      Name = name;
      Surname = surname;
      Birthdate = birthdate;
      Address = address;
      Type = type;
      ImageName = imageName;
      State = (int)EUserState.Unconfirmed;
    }

    public User()
    {
      Email = "";
      Password = "";
      Username = "";
      Name = "";
      Surname = "";
      Birthdate = "";
      Address = "";
      Type = "";
      ImageName = "";
      State = (int)EUserState.Unconfirmed;
    }

    [Key]
    public string Email       { get; set; } = String.Empty;
    public string Password    { get; set; } = String.Empty;

    public string Username    { get; set; } = String.Empty;
    public string Name        { get; set; } = String.Empty;
    public string Surname     { get; set; } = String.Empty;
    public string Birthdate
    {
      get { return _birthDate; }
      set
      {
        if (value.Contains("/")) _birthDate = value.Replace('/', '-');
      }
    }
    public string Address     { get; set; } = String.Empty;
    public string Type        { get; set; } = String.Empty;
    public string ImageName
    {
      get { return _imgName; }
      set
      {
        if (value.Equals(String.Empty)) _imgName = _DEFAULT_IMG_NAME;
      }
    }
    public int State { get; set; }
    public bool ValidateSelf(out string errStr)
    {
      errStr = "";
      DateTime dummyResult = new DateTime();
      if (this.Email == "" || this.Password == "" || this.Surname == "" || this.Address == "" || this.Type == "" || this.Username == "" || this.Name == "" || this.Birthdate == "")
        errStr += "Empty fields detected.\n";
      else
      {
        if (!DateTime.TryParseExact(this.Birthdate, "YYYY-MM-DD", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dummyResult))
          errStr += "Bad date format (must be YYYY-MM-DD).\n";

        if (this.Password.Length < 8) errStr += "Password must not be shorter than 8 characters\n";

        if (this.Username.Length < 8) errStr += "Username must not be shorter than 8 characters\n";

        if (this.Address.Length < 8) errStr += "Address must not be shorter than 8 characters\n";

        if(!(new Regex(@"/^[a-zA-Z0-9.! #$%&'*+/=? ^_`{|}~-]+@[a-zA-Z0-9-]+(?:\. [a-zA-Z0-9-]+)*$/")).IsMatch(this.Email))
          errStr += "Invalid Email format (examle: somename@somedomain.com)\n";

        if ((EUserState)this.State != EUserState.Confirmed) errStr += "Invalid initial state (must be unconfirmed)\n";
      }


      errStr.TrimEnd('\n');
      return errStr!="";
    }

  }
}
