using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Endpoints.Orders.Responses;
using reymani_web_api.Endpoints.OrdersItems.Response;

namespace reymani_web_api.Endpoints.Orders;

public class SearchOrderEndpoint : Endpoint<SearchOrderRequest, Results<Ok<PaginatedResponse<OrderResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchOrderEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/orders/search");
    Summary(s =>
    {
      s.Summary = "Search orders";
      s.Description = "Search for orders by name or ID with filtering, sorting, and pagination.";
    });
    AllowAnonymous();
  }

  public override async Task<Results<Ok<PaginatedResponse<OrderResponse>>, ProblemDetails>> ExecuteAsync(SearchOrderRequest req, CancellationToken ct)
  {
    var query = _dbContext.Orders
        .AsNoTracking()
        .Include(p => p.Items)
        .AsQueryable();

    // Filtrado por IDs
    if (req.Ids?.Any() ?? false)
      query = query.Where(pc => req.Ids.Contains(pc.Id));

    // Filtrado por Estado
    if (req.Status?.Any() ?? false)
      query = query.Where(pc => req.Status.Contains(pc.Status));

    // Filtrado por CourierId
    if (req.CourierId?.Any() ?? false)
      query = query.Where(pc => req.CourierId.Contains(pc.CourierId));

    // Filtrado por CustomerId
    if (req.CustomerId?.Any() ?? false)
      query = query.Where(pc => req.CustomerId.Contains(pc.CustomerId));


    // Ordenamiento en la base de datos
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      query = req.IsDescending ?? false
          ? query.OrderByDescending(pc => EF.Property<object>(pc, req.SortBy)) // Ordenamiento dinámico
          : query.OrderBy(pc => EF.Property<object>(pc, req.SortBy));
    }

    // Conteo total (sin paginación)
    var totalCount = await query.CountAsync(ct);

    // Paginación
    var data = await query
        .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
        .Take(req.PageSize ?? 10)
        .ToListAsync(ct);

    // Mapeo de respuesta
    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();
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

    return TypedResults.Ok(new PaginatedResponse<OrderResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
