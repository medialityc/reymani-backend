using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ShoppingCartItems.Requests;

namespace reymani_web_api.Endpoints.ShoppingCartItems;

public class DeleteShoppingCartItemEndpoint : Endpoint<DeleteShoppingCartItemRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public DeleteShoppingCartItemEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Delete("/shoppingcartitem/{id}");
    Summary(s =>
    {
      s.Summary = "Delete a shopping cart item";
      s.Description = "Deletes a  shopping cart item.";
    });
    //Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(DeleteShoppingCartItemRequest req, CancellationToken ct)
  {
    //Verifica si el carrito existe
    var existingShoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == req.ShoppingCartId, ct);

    if (existingShoppingCart == null)
      return TypedResults.NotFound();


    // Verifica si el objeto del carrito existe
    var item = await _dbContext.ShoppingCartItems.FirstOrDefaultAsync(x => x.Id == req.Id && x.ShoppingCartId == req.ShoppingCartId, ct);
    if (item is null)
      return TypedResults.NotFound();
    


    // Elimina el objeto
    _dbContext.ShoppingCartItems.Remove(item);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
