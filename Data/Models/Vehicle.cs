using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class Vehicle : BaseEntity
  {
    public int Id { get; set; }
    public int UserId { get; set; } // Courier
    public required User User { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required bool IsAvailable { get; set; }
    public required bool IsActive { get; set; }
    public required string? Picture { get; set; }
    public int VehicleTypeId { get; set; }
    public required VehicleType VehicleType { get; set; }
  }
}