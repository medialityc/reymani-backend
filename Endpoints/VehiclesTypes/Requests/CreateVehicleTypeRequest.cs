using FastEndpoints;

namespace reymani_web_api.Endpoints.VehiclesTypes.Requests;

public class CreateVehicleTypeRequest
{
  public required string Name { get; set; }
  public required int TotalCapacity { get; set; }
  public required bool IsActive { get; set; }
  public IFormFile? Logo { get; set; }
}
