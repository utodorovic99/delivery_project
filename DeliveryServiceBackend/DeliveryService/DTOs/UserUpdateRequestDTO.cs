namespace DeliveryService.DTOs
{
  public class UserUpdateRequestDTO:UserRequestDTO
  {
    public UserUpdateRequestDTO(string email, string password, string newPassword, string username, string name,
                               string surname, string birthdate, string address, string type,
                               byte[] imageRaw) : base(email, password, username, name, surname, birthdate, address, type, imageRaw)
    {
      this.NewPassword = newPassword;
    }

    public string NewPassword { get; set; }

    public UserUpdateRequestDTO() : base() { this.NewPassword = ""; }
  }
}
