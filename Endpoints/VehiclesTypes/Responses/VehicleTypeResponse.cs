

using reymani_web_api.Endpoints.ShippingsCost.Responses;
using reymani_web_api.Endpoints.ShippingsCosts.Responses;

namespace reymani_web_api.Endpoints.VehiclesTypes.Responses;

public class VehicleTypeResponse
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required string? Logo { get; set; }
  public required int TotalCapacity { get; set; }
  public required bool IsActive { get; set; }
  public ICollection<ShippingCostVehicleTypeResponse>? ShippingCosts { get; set; }
}
