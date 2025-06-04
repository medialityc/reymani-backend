using reymani_web_api.Data.Models;

namespace reymani_web_api.Endpoints.Orders.Requests;

public class CreateOrderRequest
{
  public required int ShoppingCartId { get; set; }
  public required int CustomerId { get; set; }
  public required int CustomerAddressId { get; set; }
  public required PaymentMethod PaymentMethod { get; set; }
  public required int CourierId { get; set; }
}
