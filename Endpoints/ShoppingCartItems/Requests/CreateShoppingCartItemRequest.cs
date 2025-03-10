namespace reymani_web_api.Endpoints.ShoppingCartItems.Requests;

public class CreateShoppingCartItemRequest
{
  public required int ShoppingCartId { get; set; }
  public required int ProductId { get; set; }
  public required int Quantity { get; set; }
}
