using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Endpoints.Orders.Responses;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;
using reymani_web_api.Endpoints.Products.Responses;

using ReymaniWebApi.Data.Models;
using System.Collections.Generic;

namespace reymani_web_api.Endpoints.Mappers;

public class OrderMapper
{
  public Order ToEntity(CreateOrderRequest req)
  {
    return new Order
    {
      CourierId = req.CourierId,
      ShippingCost = 0,//No tiene mensajero no tiene vehiculo no se calcula cuando se crea
      PaymentMethod = req.PaymentMethod,
      CustomerAddressId = req.CustomerAddressId,
      CustomerId = req.CustomerId,
      Status = Data.Models.OrderStatus.InProcess
    };
  }

  public OrderResponse FromEntity(Order o)
  {
    // Crear instancias de mappers para las entidades relacionadas
    var userMapper = new UserMapper();
    var userAddressMapper = new UserAddressMapper();
    var productMapper = new ProductMapper();

    return new OrderResponse
    {
      Id = o.Id,
      CourierId = o.CourierId,
      CustomerAddressId = o.CustomerAddressId,
      CustomerId = o.CustomerId,
      PaymentMethod = o.PaymentMethod,
      Status = o.Status,
      ShippingCost = o.ShippingCost,
      TotalProductsCost = o.TotalProductsCost,
      // Mapear entidades relacionadas si existen
      Customer = o.Customer != null ? userMapper.FromEntity(o.Customer) : null,
      Courier = o.Courier != null ? userMapper.FromEntity(o.Courier) : null,
      CustomerAddress = o.CustomerAddress != null ? userAddressMapper.FromEntity(o.CustomerAddress) : null,
      Items = o.Items?.Select(i => new OrderItemResponse
      {
        Id = i.Id,
        OrderId = i.OrderId, // Corregido: debe ser OrderId en lugar de Id
        Quantity = i.Quantity,
        ProductStatus = i.Status,
        // Mapear el producto si existe
        Product = i.Product != null ? productMapper.ToResponse(
          i.Product, 
          i.Product.Business?.Name ?? "Desconocido",
          i.Product.Category?.Name ?? "Desconocido",
          i.Product.Images?.ToList() ?? new List<string>(),
          0, // numberOfRatings - podría calcularse si se necesita
          0  // averageRating - podría calcularse si se necesita
        ) : null
      }).ToList()
    };
  }
}
