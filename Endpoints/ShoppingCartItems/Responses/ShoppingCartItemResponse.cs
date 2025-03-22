using reymani_web_api.Endpoints.Products.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ShoppingCartItems.Responses;

public class ShoppingCartItemResponse
{
  public int Id { get; set; }
  public required int ShoppingCartId { get; set; }
  public int ProductId { get; set; }
  public required int Quantity { get; set; }
  public ProductResponse? Product { get; set; }
}
