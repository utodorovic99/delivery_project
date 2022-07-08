using DeliveryService.Common;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DeliveryService.Model
{
  public class User
  {

    public static readonly string DEFAULT_IMG_NAME= @"select_image.png";
    private string _imgName = "";

    public User(string email, string password, string username, string name, string surname, string birthdate, string address, char type, string imageName)
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
      Type = 'n';
      ImageName = "";
      State = (int)EUserState.Unconfirmed;
    }

    public string Email       { get; set; } = String.Empty;
    public string Password    { get; set; } = String.Empty;

    public string Username    { get; set; } = String.Empty;
    public string Name        { get; set; } = String.Empty;
    public string Surname     { get; set; } = String.Empty;
    public string Birthdate   { get; set; } = String.Empty;
    public string Address     { get; set; } = String.Empty;
    public char Type        { get; set; }                     //Deliveryman(d), Administrator(a), Consumer(c)
    public string ImageName
    {
      get { return _imgName; }
      set
      {
        if (value.Equals(String.Empty)) _imgName = DEFAULT_IMG_NAME;
      }
    }
    public int State { get; set; }                            //Not Verified, Pending, Verified
  }
}
