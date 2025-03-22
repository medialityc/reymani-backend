using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Endpoints.Orders.Responses;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class OrderMapper
{
  public Order ToEntity(CreateOrderRequest req)
  {
    return new Order
    {
      CourierId = 0,//No tiene mensajero asignado
      ShippingCost =0,//No tiene mensajero no tiene vehiculo no se calcula cuando se crea
      PaymentMethod = req.PaymentMethod,
      CustomerAddressId = req.CustomerAddressId,
      CustomerId = req.CustomerId,
      Status = Data.Models.OrderStatus.InProcess
    };
  }

  public OrderResponse FromEntity(Order o)
  {
    return new OrderResponse
    {
      CourierId = o.CourierId,
      CustomerAddressId = o.CustomerAddressId,
      CustomerId = o.CustomerId,
      PaymentMethod = o.PaymentMethod,
      Status = o.Status,
      Items = o.Items?.Select(i => new OrderItemResponse
      {
        Id = i.Id,
        OrderId = i.Id,
        Quantity = i.Quantity,
        ProductStatus = i.Status
      }).ToList()
    };
  }
}
