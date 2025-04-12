﻿

namespace reymani_web_api.Endpoints.Vehicles.Response;

public class VehicleResponse
{
  public int Id { get; set; }
  public required int UserId { get; set; } // Courier
  public required string Name { get; set; }
  public required string? Picture { get; set; }
  public required string? Description { get; set; }
  public required int VehicleTypeId { get; set; }
  public required string VehicleTypeName { get; set; }
  public  bool IsAvailable { get; set; }
  public  bool IsActive { get; set; }
}
