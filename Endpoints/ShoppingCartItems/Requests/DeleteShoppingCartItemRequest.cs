namespace reymani_web_api.Endpoints.ShoppingCartItems.Requests;

public class DeleteShoppingCartItemRequest
{
  public required int Id { get; set; }
  public required int ShoppingCartId { get; set; }
}
