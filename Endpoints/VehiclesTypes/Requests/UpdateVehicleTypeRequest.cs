namespace reymani_web_api.Endpoints.VehiclesTypes.Requests;

public class UpdateVehicleTypeRequest
{
  public required int Id { get; set; }
  public required string? Name { get; set; }
  public required int? TotalCapacity { get; set; }
  public required bool? IsActive { get; set; }
  public required IFormFile? Logo { get; set; }
}
