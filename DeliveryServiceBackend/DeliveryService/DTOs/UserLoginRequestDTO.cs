namespace DeliveryService.DTOs
{
  public class UserLoginRequestDTO
  {
    public UserLoginRequestDTO(string email, string password)
    {
      Email = Email;
      Password = password;
    }

    public UserLoginRequestDTO()
    {
      Email = "";
      Password = "";
    }

    public string Email { get; set; }
    public string Password { get; set; }
  }
}
