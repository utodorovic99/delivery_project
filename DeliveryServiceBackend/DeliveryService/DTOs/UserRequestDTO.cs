namespace DeliveryService.DTOs
{
  public abstract class UserRequestDTO
  {
    protected UserRequestDTO(string email, string password, string username, string name, string surname, string birthdate, string address, string type, IFormFile imageRaw)
    {
      Email = email;
      Password = password;
      Username = username;
      Name = name;
      Surname = surname;
      Birthdate = birthdate;
      Address = address;
      Type = type;
      ImageRaw = imageRaw;
    }

    protected UserRequestDTO()
    {
      Email = "";
      Password = "";
      Username = "";
      Name = "";
      Surname = "";
      Birthdate = "";
      Address = "";
      Type = "";
      ImageRaw = null;
    }

    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string Birthdate { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
    public IFormFile ImageRaw { get; set; }
  }
}
