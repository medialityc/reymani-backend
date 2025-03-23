namespace reymani_web_api.Endpoints.Orders.Requests;

public class ConfirmOrderRequest
{
  public required int OrderId { get; set; }
  public required int CourierId { get; set; }
}
