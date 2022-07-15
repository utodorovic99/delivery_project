namespace DeliveryService.DTOs
{
  public class UserUpdateRequestDTO
  {
    public UserUpdateRequestDTO(string email, string password, string newPassword, string username, string name, string surname, string birthdate, string address, string type)
    {
      this.Email = email;
      this.Username = username;
      this.Password=password;
      this.Name = name;
      this.Surname= surname;
      this.Birthdate = birthdate;
      this.Address = address;
      this.Type= type;
      this.NewPassword = newPassword;
    }

    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string Birthdate { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
    public string NewPassword { get; set; } = String.Empty;

    public UserUpdateRequestDTO()
    {
      this.Email = "";
      this.Username = "";
      this.Password = "";
      this.Name = "";
      this.Surname = "";
      this.Birthdate = "";
      this.Address = "";
      this.Type = "";
      this.NewPassword = "";
    }
  }
}
