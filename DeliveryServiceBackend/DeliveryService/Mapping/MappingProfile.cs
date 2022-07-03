using AutoMapper;
using DeliveryService.DTOs;
using DeliveryService.Model;

namespace DeliveryService.Mapping
{
  public class MappingProfile:Profile
  {
    public MappingProfile()
    {
      CreateMap<Order, OrderDTO>().ReverseMap();

      CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

      CreateMap<Product, ProductDTO>().ReverseMap();

      CreateMap<User, UserDTO>().ReverseMap();
    }
  }
}
