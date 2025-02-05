using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class ShippingCost : BaseEntity
  {
    public int Id { get; set; }
    public int VehicleTypeId { get; set; }
    public required VehicleType VehicleType { get; set; }
    public int MunicipalityId { get; set; }
    public required Municipality Municipality { get; set; }
    public required decimal Cost { get; set; }
  }
}