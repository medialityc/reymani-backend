using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.ShoppingCartItems.Requests;
using reymani_web_api.Endpoints.ShoppingCartItems.Responses;

namespace reymani_web_api.Endpoints.ShoppingCartItems;

public class CreateShoppingCartItemEndpoint : Endpoint<CreateShoppingCartItemRequest, Results<Created<ShoppingCartItemResponse>, Conflict,NotFound, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public CreateShoppingCartItemEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Post("/shoppingcartitem");
    Summary(s =>
    {
      s.Summary = "Create a shopping cart item";
      s.Description = "Creates a new shopping cart item.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Created<ShoppingCartItemResponse>, Conflict, NotFound,UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateShoppingCartItemRequest req, CancellationToken ct)
  {
    var existingShoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == req.ShoppingCartId, ct);

    if (existingShoppingCart == null)
      return TypedResults.NotFound();


    var existingItem = await _dbContext.ShoppingCartItems.FirstOrDefaultAsync(x => x.ShoppingCartId == req.ShoppingCartId && x.Id == req.ProductId, ct);
    if (existingItem != null)
      return TypedResults.Conflict();
    

    var mapper = new ShoppingCartItemMapper();
    var item = mapper.ToEntity(req);

    // Agrega la nueva provincia a la base de datos
    _dbContext.ShoppingCartItems.Add(item);
    await _dbContext.SaveChangesAsync(ct);


    var response = mapper.FromEntity(item);


    return TypedResults.Created($"/shoppingcart/item/{item.Id}", response);
  }
}
