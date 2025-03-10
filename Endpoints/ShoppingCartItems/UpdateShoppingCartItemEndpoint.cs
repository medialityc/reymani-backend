using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.ShoppingCartItems.Requests;

namespace reymani_web_api.Endpoints.ShoppingCartItems;

public class UpdateShoppingCartItemEndpoint : Endpoint<UpdateShoppingCartItemRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public UpdateShoppingCartItemEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Put("/shoppingcartitem/{id}");
    Summary(s =>
    {
      s.Summary = "Update shopping cart item";
      s.Description = "Updates details of an existing shopping cart item.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateShoppingCartItemRequest req, CancellationToken ct)
  {
    //Verifica si el carrito existe
    var existingShoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == req.ShoppingCartId, ct);

    if (existingShoppingCart == null)
      return TypedResults.NotFound();

    // Verifica si el objeto existe
    var item = await _dbContext.ShoppingCartItems.FindAsync(req.Id, ct);
    if (item is null)
    {
      return TypedResults.NotFound();
    }

    // Actualiza la cantidad
    item.Quantity = req.Quantity;
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

}
