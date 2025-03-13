using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data.Models;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.Responses;
using reymani_web_api.Endpoints.OrdersItems.Response;
using ReymaniWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;

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

    var response = orders.Select(u => mapper.FromEntity(u)).ToList();

    foreach (var r in response)
    {
      ICollection<OrderItemResponse> itemsResponse = new List<OrderItemResponse>();
      foreach (var ir in r.Items!)
      {
        itemsResponse.Add(mapperItem.FromEntity(ir));
      }

      r.Items.Clear();
      r.Items.Concat(itemsResponse);
    }
    return TypedResults.Ok(response.AsEnumerable());
  }
}
