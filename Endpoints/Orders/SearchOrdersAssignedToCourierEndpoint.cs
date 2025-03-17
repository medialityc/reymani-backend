using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data.Models;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.Responses;
using reymani_web_api.Endpoints.Orders.Requests;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Endpoints.OrdersItems.Response;
using reymani_web_api.Endpoints.Commons.Responses;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Orders;

public class SearchOrdersAssignedToCourierEndpoint : Endpoint<SearchOrdersAssignedToCourierRequest, Results<Ok<PaginatedResponse<OrderResponse>>, UnauthorizedHttpResult, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchOrdersAssignedToCourierEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/orders/search-assigned-to-courier");
    Summary(s =>
    {
      s.Summary = "Search orders assigned to the authenticated courier";
      s.Description = "Retrieves a paginated list of orders with items assigned to the authenticated courier that are ready for pickup, not yet picked up, and not in Delivered, Completed, or Canceled states.";
    });
    Roles("Courier");
  }

  public override async Task<Results<Ok<PaginatedResponse<OrderResponse>>, UnauthorizedHttpResult, NotFound, ProblemDetails>> ExecuteAsync(SearchOrdersAssignedToCourierRequest req, CancellationToken ct)
  {
    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    // Verificar si el mensajero está autenticado
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int courierId))
      return TypedResults.Unauthorized();

    // Base query para obtener los pedidos asignados al mensajero
    var query = _dbContext.Orders
        .Include(o => o.Items!)
        .ThenInclude(i => i.Product)
        .Where(o => o.CourierId == courierId && o.Items!
            .Any(i => 
                       i.Status == OrderStatus.InPickup && // Listos para recoger
                       i.Status == OrderStatus.InPickup && // Aún no recogidos
                       o.Status != OrderStatus.Delivered && // No entregados
                       o.Status != OrderStatus.Completed && // No finalizados
                       o.Status != OrderStatus.Cancelled)) // No cancelados
        .AsNoTracking();

    // Ordenamiento dinámico
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      query = req.IsDescending ?? false
          ? query.OrderByDescending(o => EF.Property<object>(o, req.SortBy)) // Orden descendente
          : query.OrderBy(o => EF.Property<object>(o, req.SortBy)); // Orden ascendente
    }
    else
    {
      // Ordenamiento por defecto (por ID del pedido)
      query = query.OrderBy(o => o.Id);
    }

    // Conteo total de pedidos (sin paginación)
    var totalCount = await query.CountAsync(ct);

    // Paginación
    var data = await query
        .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10)) // Cálculo del salto
        .Take(req.PageSize ?? 10) // Tamaño de la página
        .ToListAsync(ct);

    // Si no se encuentran pedidos, devolver 404 Not Found
    if (data == null || !data.Any())
      return TypedResults.NotFound();

    // Mapear las entidades Order a OrderResponse
    var responseData = data.Select(u => mapper.FromEntity(u)).ToList();

    foreach (var r in responseData)
    {
      ICollection<OrderItemResponse> itemsResponse = new List<OrderItemResponse>();
      foreach (var ir in r.Items!)
      {
        itemsResponse.Add(mapperItem.FromEntity(ir));
      }

      r.Items.Clear();
      r.Items.Concat(itemsResponse);
    }

    // Respuesta paginada
    return TypedResults.Ok(new PaginatedResponse<OrderResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}