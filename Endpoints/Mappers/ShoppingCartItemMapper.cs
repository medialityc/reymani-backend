using reymani_web_api.Endpoints.Products.Responses;
using reymani_web_api.Endpoints.ShoppingCartItems.Requests;
using reymani_web_api.Endpoints.ShoppingCartItems.Responses;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class ShoppingCartItemMapper
{
  public ShoppingCartItemResponse FromEntity(ShoppingCartItem e)
  {
    return new ShoppingCartItemResponse
    {
     Id = e.Id,
     ShoppingCartId = e.ShoppingCartId,
     ProductId = e.ProductId,
     Quantity = e.Quantity,
     Product = null
    };
  }

  public ShoppingCartItem ToEntity(CreateShoppingCartItemRequest req)
  {
    return new ShoppingCartItem
    {
      ShoppingCartId = req.ShoppingCartId,
      ProductId = req.ProductId,
      Quantity = req.Quantity
    };
  }
}
