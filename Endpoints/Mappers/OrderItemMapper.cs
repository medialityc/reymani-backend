using reymani_web_api.Endpoints.Orders.OrdersItems.Response;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class OrderItemMapper
{
  public OrderItem OrderItemFromShoppingCartItem(ShoppingCartItem e, Order o)
  {
    return new OrderItem
    {
      Id = e.Id,
      OrderId = o.Id,
      Quantity = e.Quantity,
      ProductId = e.ProductId,
    };
  }

  public OrderItemResponse FromEntity(OrderItemResponse item)
  {
    return new OrderItemResponse
    {
      OrderId = item.Id,
      Quantity = item.Quantity,
      Product = item.Product,
      ProductStatus = item.ProductStatus      
    };
  }
}
