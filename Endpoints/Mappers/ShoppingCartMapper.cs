using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.ShoppingCarts.Requests;
using reymani_web_api.Endpoints.ShoppingCarts.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class ShoppingCartMapper
{
  public ShoppingCartResponse FromEntity(ShoppingCart e)
  {
    return new ShoppingCartResponse
    {
      Id = e.Id,
      UserAddressId = e.Id,
      UserId = e.UserId,
    };
  }

  public ShoppingCart ToEntity(CreateShoppingCartRequest req)
  {
    return new ShoppingCart
    {
      UserAddressId = req.UserAddressId,
      UserId = req.UserId
    };
  }
}
