namespace reymani_web_api.Endpoints.ShippingsCosts.Responses;

public class ShippingCostVehicleTypeResponse
{
  public int Id { get; set; }
  public required int MunicipalityId { get; set; }
  public required string MunicipalityName { get; set; }
  public required decimal Cost { get; set; }
}
