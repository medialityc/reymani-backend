using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Users.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ShoppingCarts.Responses;

public class ShoppingCartResponse
{
  public int Id { get; set; }
  //public ICollection<ShoppingCartItem>? Items { get; set; }
  public required int UserId { get; set; } // Customer User
  public required int UserAddressId { get; set; }
}
