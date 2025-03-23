using reymani_web_api.Endpoints.ShoppingCartItems.Responses;


using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ShoppingCarts.Responses;

public class ShoppingCartResponse
{
  public int Id { get; set; }
  public ICollection<ShoppingCartItemResponse>? Items { get; set; }
  public required int UserId { get; set; } // Customer User
  public required int UserAddressId { get; set; }
}
