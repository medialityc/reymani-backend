using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.ShoppingCartItems.Responses;
using reymani_web_api.Endpoints.ShoppingCarts.Requests;


using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ShoppingCarts;

public class SearchItemInShoppingCartEndpoint : Endpoint<SearchItemInShoppingCartRequest, Results<Ok<PaginatedResponse<ShoppingCartItemResponse>>,UnauthorizedHttpResult ,ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchItemInShoppingCartEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/shoppingcart/search");
    Summary(s =>
    {
      s.Summary = "Search products in shoppingcart";
      s.Description = "Search for products by name or ID with filtering, sorting, and pagination.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok<PaginatedResponse<ShoppingCartItemResponse>>, UnauthorizedHttpResult,ProblemDetails>> ExecuteAsync(SearchItemInShoppingCartRequest req, CancellationToken ct)
  {
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      return TypedResults.Unauthorized();

    var query = _dbContext.ShoppingCartItems
        .AsNoTracking()
        .Where(p=> p.ShoppingCart!.UserId == userId)
        .Include(p => p.Product)
        .AsQueryable();

    // Filtrado
    if (req.Names?.Any() ?? false)
      query = query.Where(pc => req.Names.Any(n => pc.Product!.Name.ToLower().Contains(n!.ToLower().Trim())));

    if (req.Search is not null)
    {
      var search = req.Search.ToLower().Trim();
      query = query.Where(pc => pc.Product!.Name.ToLower().Contains(search));
    }

    if (req.PriceMin.HasValue)
      query = query.Where(pc => pc.Product!.Price >= req.PriceMin.Value);

    // Filtrado por capacidad máxima
    if (req.PriceMax.HasValue)
      query = query.Where(pc => pc.Product!.Price <= req.PriceMax.Value);


    // Ejecución de la consulta (sin ordenamiento)
    var items = await query.ToListAsync(ct);

    // Ordenamiento en el cliente
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      var propertyInfo = typeof(ShoppingCartItem).GetProperty(req.SortBy);
      if (propertyInfo != null)
      {
        items = req.IsDescending ?? false
            ? items.OrderByDescending(p => propertyInfo.GetValue(p)).ToList()
            : items.OrderBy(p => propertyInfo.GetValue(p)).ToList();
      }
    }

    // Paginación en el cliente
    var totalCount = items.Count;
    var data = items
        .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
        .Take(req.PageSize ?? 10)
        .ToList();

    // Mapeo de respuesta
    var mapper = new ShoppingCartItemMapper();
    var responseData = data.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(new PaginatedResponse<ShoppingCartItemResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
