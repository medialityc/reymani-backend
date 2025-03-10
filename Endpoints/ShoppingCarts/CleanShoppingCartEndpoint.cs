using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ShoppingCarts.Requests;

namespace reymani_web_api.Endpoints.ShoppingCarts;

public class CleanShoppingCartEndpoint : Endpoint<CleanShoppingCartRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public CleanShoppingCartEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Delete("/shoppingcart/{id}");
    Summary(s =>
    {
      s.Summary = "Clean a shopping cart";
      s.Description = "Clean a  shopping cart.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CleanShoppingCartRequest req, CancellationToken ct)
  {
    //Verifica si el carrito existe
    var existingShoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

    if (existingShoppingCart == null)
      return TypedResults.NotFound();

    if(existingShoppingCart.Items != null)
      existingShoppingCart.Items.Clear();
    
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
