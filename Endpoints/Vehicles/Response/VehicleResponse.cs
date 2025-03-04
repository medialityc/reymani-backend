using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Vehicles.Response;

public class VehicleResponse
{
  public int Id { get; set; }
  public required int UserId { get; set; } // Courier
  public required string Name { get; set; }
  public required string? Picture { get; set; }
  public required string VehicleTypeName { get; set; }
}
