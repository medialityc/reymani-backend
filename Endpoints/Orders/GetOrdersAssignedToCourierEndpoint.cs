using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data.Models;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;

namespace reymani_web_api.Endpoints.Orders;

public class GetOrdersAssignedToCourierEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<OrderResponse>>, UnauthorizedHttpResult, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public GetOrdersAssignedToCourierEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/orders/assigned-to-courier");
    Summary(s =>
    {
      s.Summary = "Get orders assigned to the authenticated courier";
      s.Description = "Retrieves a list of orders with items assigned to the authenticated courier that are ready for pickup, not yet picked up, and not in Delivered, Completed, or Canceled states.";
    });
    Roles("Courier");
  }

  public override async Task<Results<Ok<IEnumerable<OrderResponse>>, UnauthorizedHttpResult, NotFound, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    // Verificar si el mensajero está autenticado
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int courierId))
      return TypedResults.Unauthorized();

    // Obtener los pedidos que tienen ítems asignados al mensajero autenticado
    var orders = await _dbContext.Orders
        .Include(o => o.Items!)
        .ThenInclude(i => i.Product)
        .Where(o => o.CourierId==courierId && o.Items! 
            .Any(i => i.Status == OrderStatus.InPickup && // Listos para recoger
                       o.Status != OrderStatus.Delivered && // No entregados
                       o.Status != OrderStatus.Completed && // No finalizados
                       o.Status != OrderStatus.Cancelled)) // No cancelados
        .AsNoTracking()
        .OrderBy(o => o.Id)
        .ToListAsync(ct);

    // Si no se encuentran pedidos, devolver 404 Not Found
    if (orders == null || !orders.Any())
      return TypedResults.NotFound();

    // Mapear las entidades Order a OrderResponse
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