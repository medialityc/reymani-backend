using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Products.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Orders.OrdersItems.Response;

public class OrderItemResponse
{
  public int Id { get; set; }
  public required int OrderId { get; set; }
  public ProductResponse? Product { get; set; }
  public required OrderItemStatus ProductStatus { get; set; }
  public required int Quantity { get; set; }
}
