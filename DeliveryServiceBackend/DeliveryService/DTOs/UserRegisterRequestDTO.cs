using DeliveryService.Common;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DeliveryService.DTOs
{
  public class UserRegisterRequestDTO:UserRequestDTO
  {
    public UserRegisterRequestDTO(string email, string password, string username, string name,
                               string surname, string birthdate, string address, string type,
                               IFormFile imageRaw):base(email, password, username, name, surname, birthdate, address, type, imageRaw) {}

   
    public UserRegisterRequestDTO():base(){}
    
  }
}
