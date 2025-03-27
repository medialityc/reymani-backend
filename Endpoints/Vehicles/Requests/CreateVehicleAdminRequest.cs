namespace reymani_web_api.Endpoints.Vehicles.Requests;

public class CreateVehicleAdminRequest
{
  public required int UserId { get; set; }
  public required string Name { get; set; }
  public required string? Description { get; set; }
  public required bool IsAvailable { get; set; }
  public required bool IsActive { get; set; }
  public required IFormFile Picture { get; set; }
  public required int VehicleTypeId { get; set; }
}
