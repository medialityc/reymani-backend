namespace reymani_web_api.Endpoints.OrdersItems.Requests;

public class ConfirmPickUpOrderItemRequest
{
  public required int OrderId { get; set; }
  public required int OrderItemId { get; set; }
}
