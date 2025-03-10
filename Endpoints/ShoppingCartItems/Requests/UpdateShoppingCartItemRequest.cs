namespace reymani_web_api.Endpoints.ShoppingCartItems.Requests;

public class UpdateShoppingCartItemRequest
{
  public required int Id { get; set; }
  public required int ShoppingCartId {get;set;}
  public required int Quantity { get; set; }
}
