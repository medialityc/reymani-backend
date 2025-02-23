using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.AddressUser.Requests;

public class CreateCustomerRequest
{
  public required int UserId { get; set; }
  public required string Name { get; set; }
  public required string? Notes { get; set; }
  public required string Address { get; set; }
  public required int MunicipalityId { get; set; }
  public required bool IsActive { get; set; }
}
