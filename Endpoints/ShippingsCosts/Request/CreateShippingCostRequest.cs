namespace reymani_web_api.Endpoints.ShippingCost.Request;

public class CreateShippingCostRequest
{
  public required int MunicipalityId { get; set; }
  public required int VehicleTypeId { get; set; }
  public required decimal Cost { get; set; }
}
