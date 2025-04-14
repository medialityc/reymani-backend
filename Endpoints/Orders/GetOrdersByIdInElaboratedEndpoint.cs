﻿using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Endpoints.Orders.Responses;

namespace reymani_web_api.Endpoints.Orders;
public class GetOrderByIdInElaboratedEndpoint : Endpoint<GetOrderByIdInElaboratedRequest, Results<Ok<OrderResponse>, UnauthorizedHttpResult, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public GetOrderByIdInElaboratedEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/orders/elaborate/{id}");
    Summary(s =>
    {
      s.Summary = "Get a specific order by ID in elaboration state";
      s.Description = "Retrieves a specific order by ID that is in elaboration state and contains items from the authenticated business admin's businesses.";
    });
    Roles("BusinessAdmin");
  }

  public override async Task<Results<Ok<OrderResponse>, UnauthorizedHttpResult, NotFound, ProblemDetails>> ExecuteAsync(GetOrderByIdInElaboratedRequest req, CancellationToken ct)
  {
    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    // Verificar si el usuario está autenticado
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

    // Obtener la orden por ID que tenga ítems en estado "InPreparation" y que esos ítems pertenezcan a los productos de los negocios del usuario
    var order = await _dbContext.Orders
        .AsNoTracking()
        .Include(o => o.Items!)
            .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Category)
        .Include(o => o.Items!)
            .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Business)
        .Include(o => o.Customer)
        .Include(o => o.Courier)
        .Include(o => o.CustomerAddress)
            .ThenInclude(ca => ca.Municipality)
                .ThenInclude(m => m.Province)
        .FirstOrDefaultAsync(o => o.Id == req.Id && o.Items!
            .Any(i => i.Status == OrderItemStatus.InPreparation && productIds.Contains(i.ProductId)), ct);

    // Si no se encuentra la orden, devolver 404 Not Found
    if (order == null)
      return TypedResults.NotFound();

    // Mapear la entidad Order a OrderResponse
    var response = mapper.FromEntity(order);

    // Mapear los ítems de la orden directamente desde la entidad order
    // en lugar de usar los items ya mapeados en response
    var itemsResponse = order.Items!.Select(item => mapperItem.FromEntity(item)).ToList();
    response.Items = itemsResponse;

    return TypedResults.Ok(response);
  }
}