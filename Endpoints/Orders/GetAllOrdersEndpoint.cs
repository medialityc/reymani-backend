using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.Responses;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;

namespace reymani_web_api.Endpoints.Orders;

public class GetAllOrdersEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<OrderResponse>>, UnauthorizedHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public GetAllOrdersEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/orders/all");
    Summary(s =>
    {
      s.Summary = "Get all order and product";
      s.Description = "Retrieves a list of all orders.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<IEnumerable<OrderResponse>>, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();

    // Obtener las órdenes
    var orders = await _dbContext.Orders
        .Include(o => o.Items!)
        .ThenInclude(i => i.Product)
        .AsNoTracking()
        .ToListAsync(ct);

    // Mapear las entidades Order a OrderResponse con los items directamente desde las entidades
    var response = new List<OrderResponse>();
    foreach (var order in orders)
    {
      var orderResponse = mapper.FromEntity(order);
      // Mapear los ítems de la orden directamente desde la entidad order
      var itemsResponse = order.Items!.Select(item => mapperItem.FromEntity(item)).ToList();
      orderResponse.Items = itemsResponse;
      response.Add(orderResponse);
    }

    return TypedResults.Ok(response.AsEnumerable());
  }
}
