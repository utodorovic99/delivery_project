namespace DeliveryService.DTOs
{
  public class UserLoginRequestDTO
  {
    public UserLoginRequestDTO(string username, string password)
    {
      Username = username;
      Password = password;
    }

    public UserLoginRequestDTO()
    {
      Username = "";
      Password = "";
    }

    public string Username { get; set; }
    public string Password { get; set; }
  }
}
