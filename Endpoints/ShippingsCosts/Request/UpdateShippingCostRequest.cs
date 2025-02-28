namespace reymani_web_api.Endpoints.ShippingsCosts.Request;

public class UpdateShippingCostRequest
{
  public required int Id { get; set; }
  public required decimal Cost { get; set; }
}
