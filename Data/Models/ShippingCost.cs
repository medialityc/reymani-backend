using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class ShippingCost : BaseEntity
  {
    public int Id { get; set; }
    public required int VehicleTypeId { get; set; }
    public VehicleType? VehicleType { get; set; }
    public required int MunicipalityId { get; set; }
    public Municipality? Municipality { get; set; }
    public required decimal Cost { get; set; }
  }
}