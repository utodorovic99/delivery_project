using DeliveryService.Common;
using DeliveryService.Data;
using DeliveryService.DTOs;

namespace DeliveryService.Services.Interfaces
{
  public interface ITransistentUserService
  {
    public bool TryRegister(UserRegisterRequestDTO regReq, out string errMsg);

    public bool TryLogin(UserLoginRequestDTO loginReq, out string errMsg, out string token);

    public bool TryUpdate(UserUpdateRequestDTO updateReq, string username, out string errMsg);

    public List<UserDTO> GetAll();

    public UserDTO GetByUsername(string username);

    public UserDTO GetByEmail(string email);

    public bool SetState(string username, EUserState state, out string errMsg);

    public bool ValidatePassword(string username, string passwordPlain);

    public string GetRole(string username);

    public bool IsVerified(string username);
  }
}
