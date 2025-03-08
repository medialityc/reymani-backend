namespace reymani_web_api.Endpoints.ShoppingCarts.Requests;

public class CreateShoppingCartRequest
{
  public required int UserAddressId { get; set; }
  public required int UserId { get; set; }
}
