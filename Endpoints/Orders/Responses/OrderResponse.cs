using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;
using reymani_web_api.Endpoints.Users.Responses;


namespace reymani_web_api.Endpoints.Orders.Responses;

public class OrderResponse
{
  public int Id { get; set; }
  public required PaymentMethod PaymentMethod { get; set; }
  public required int CustomerId { get; set; }
  public UserResponse? Customer { get; set; }
  public required bool RequiresCourierService { get; set; }
  public int? CourierId { get; set; }
  public UserResponse? Courier { get; set; }
  public ICollection<OrderItemResponse>? Items { get; set; }
  public required OrderStatus Status { get; set; }
  public decimal ShippingCost { get; set; }
  public decimal TotalProductsCost { get; set; }
  public required int CustomerAddressId { get; set; }
  public UserAddressResponse? CustomerAddress { get; set; }
}
