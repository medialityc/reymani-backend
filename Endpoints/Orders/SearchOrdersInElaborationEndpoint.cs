using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data.Models;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.Responses;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;



namespace reymani_web_api.Endpoints.Orders;
public class SearchOrdersInElaborationEndpoint : Endpoint<SearchOrdersInElaborationRequest, Results<Ok<PaginatedResponse<OrderResponse>>, UnauthorizedHttpResult, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchOrdersInElaborationEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/orders/search-in-elaboration");
    Summary(s =>
    {
      s.Summary = "Search orders in elaboration state with items from the authenticated business admin's business";
      s.Description = "Retrieves a paginated list of orders in elaboration state that contain items from the authenticated business admin's business and are not yet ready for pickup.";
    });
    Roles("BusinessAdmin");
  }

  public override async Task<Results<Ok<PaginatedResponse<OrderResponse>>, UnauthorizedHttpResult, NotFound, ProblemDetails>> ExecuteAsync(SearchOrdersInElaborationRequest req, CancellationToken ct)
  {
    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    // Verificar si el administrador de negocio está autenticado
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      return TypedResults.Unauthorized();

    // Obtener los negocios del administrador autenticado
    var businesses = await _dbContext.Businesses
        .Include(b => b.Products)
        .Where(b => b.UserId == userId)
        .ToListAsync(ct);

    // Obtener los IDs de los productos que pertenecen a los negocios del administrador
    var productIds = businesses
        .SelectMany(b => b.Products!)
        .Select(p => p.Id)
        .ToList();

    // Base query para obtener los pedidos en estado "En Elaboración"
    var query = _dbContext.Orders
        .AsNoTracking()
        .Include(p => p.Items!)
            .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Category)
        .Include(p => p.Items!)
            .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Business)
        .Include(p => p.Customer)
        .Include(p => p.Courier)
        .Include(p => p.CustomerAddress)
            .ThenInclude(ca => ca.Municipality)
                .ThenInclude(m => m.Province)
        .Where(o => o.Status == OrderStatus.InPreparation && // Pedidos en estado "En Elaboración"
                     o.Items!
                         .Any(i => productIds.Contains(i.ProductId) && // Ítems del negocio del administrador
                               i.Status != OrderStatus.InPickup)) // Ítems que no están listos para recoger
        .AsQueryable();

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
