using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ShippingsCost.Responses;

public class ShippingCostResponse
{
  public int Id { get; set; }
  public required int VehicleTypeId { get; set; }
  public required string VehicleName { get; set; }
  public required int MunicipalityId { get; set; }
  public required string MunicipalityName { get; set; }
  public required decimal Cost { get; set; }
}
