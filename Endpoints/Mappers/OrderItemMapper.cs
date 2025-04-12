using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;
using reymani_web_api.Endpoints.Products.Responses;

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
      Status = OrderItemStatus.InPreparation
    };
  }

  public OrderItemResponse FromEntity(OrderItem item)
  {
    var productMapper = new ProductMapper();

    return new OrderItemResponse
    {
      Id = item.Id,
      OrderId = item.OrderId,
      Quantity = item.Quantity,
      ProductStatus = item.Status,
      Product = item.Product != null ? productMapper.ToResponse(
        item.Product,
        item.Product.Business?.Name ?? "Desconocido",
        item.Product.Category?.Name ?? "Desconocido",
        item.Product.Images?.ToList() ?? new List<string>(),
        0, // numberOfRatings
        0  // averageRating
      ) : null
    };
  }
}
