namespace reymani_web_api.Endpoints.AddressUser.Requests;

public class UpdateCustomerRequest
{
  public required int Id { get; set; }
  public required string Name { get; set; }
  public required string? Note { get; set; }
  public required string Address { get; set; }
  public required bool IsActive { get; set; }
}
