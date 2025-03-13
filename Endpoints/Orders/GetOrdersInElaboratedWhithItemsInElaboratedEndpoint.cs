using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.Responses;
using reymani_web_api.Endpoints.OrdersItems.Response;


namespace reymani_web_api.Endpoints.Orders;

public class GetOrdersInElaboratedWhithItemsInElaboratedEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<OrderResponse>>, UnauthorizedHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public GetOrdersInElaboratedWhithItemsInElaboratedEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/orders/elaborateitems");
    Summary(s =>
    {
      s.Summary = "Get all order and product in elaborating";
      s.Description = "Retrieves a list of all orders.";
    });
    Roles("BusinessAdmin");
  }

  public override async Task<Results<Ok<IEnumerable<OrderResponse>>,UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      return TypedResults.Unauthorized();

    // Obtener los negocios del usuario autenticado
    var business = await _dbContext.Businesses
        .Include(b => b.Products)
        .Where(b => b.UserId == userId)
        .ToListAsync(ct);

    // Obtener los IDs de los productos que pertenecen a los negocios del usuario
    var productIds = business
        .SelectMany(b => b.Products!)
        .Select(p => p.Id)
        .ToList();

    // Obtener las órdenes que tengan ítems en estado InPreparation y que esos ítems pertenezcan a los productos de los negocios del usuario
    var orders = await _dbContext.Orders
        .Include(o => o.Items!)
        .ThenInclude(i => i.Product)
        .Where(o => o.Items!
            .Any(i => i.Status == OrderStatus.InPreparation && productIds.Contains(i.ProductId)))
        .AsNoTracking()
        .OrderBy(o => o.Id)
        .ToListAsync(ct);

    var response = orders.Select(u => mapper.FromEntity(u)).ToList();

    foreach(var r in response)
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
