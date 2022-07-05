namespace DeliveryService.DTOs
{
  public class PrimitiveResponseDTO
  {
    public PrimitiveResponseDTO(string value, string type)
    {
      Value = value;
      Type = type;
    }

    public string Value { get; set; }
    public string Type { get; set; }
  }
}
