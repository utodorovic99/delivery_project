using DeliveryService.Common;

namespace DeliveryService.DTOs
{
  public class UserDTO
  {
    public UserDTO(string email, string username, string name, string surname, string birthdate, string address, string type, byte[] imageRaw)
    {
      Email = email;
      Username = username;
      Name = name;
      Surname = surname;
      Birthdate = birthdate;
      Address = address;
      Type = type;   
      ImageRaw = imageRaw;
      State = (int)EUserState.Unconfirmed;
    }

    private string _type = "";

    public UserDTO()
    {
      Email = "";
      Username = "";
      Name = "";
      Surname = "";
      Birthdate = "";
      Address = "";
      Type = "";
      ImageRaw = new byte[1];
      ImageRaw[0] = 0;
      State = (int)EUserState.Unconfirmed;
    }

    public string Email { get; set; } 
    public string Username { get; set; } 
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Birthdate { get; set; }
    public string Address { get; set; } 
    public string Type
    {
      get { return _type; }
      set
      {
        if (new List<string> { "d", "c", "a" }.Contains(value))
        {
          switch (value)
          {
            case "d": { _type = "deliveryman"; break; }
            case "c": { _type = "consumer"; break; }
            case "a": { _type = "administrator"; break; }
          }
        }
        else if (new List<string> { "deliveryman", "consumer", "administrator" }.Contains(value))
          _type = value;
        else
          _type = "unknown";
      }
    } 
    public byte[] ImageRaw { get; set; }
    public int State { get; set; }
  }
}
