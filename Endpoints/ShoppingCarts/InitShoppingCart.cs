using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.ShoppingCarts.Requests;
using reymani_web_api.Endpoints.ShoppingCarts.Responses;

namespace reymani_web_api.Endpoints.ShoppingCarts;

public class InitShoppingCart : Endpoint<CreateShoppingCartRequest, Results<Created<ShoppingCartResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  public InitShoppingCart(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Post("/shoppingcart");
    Summary(s =>
    {
      s.Summary = "Create shopping cart";
      s.Description = "Creates a new shoping cart.";
    });
    //Roles("Customer");
    AllowAnonymous();
  }

  public override async Task<Results<Created<ShoppingCartResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateShoppingCartRequest req, CancellationToken ct)
  {
    var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(p => p.UserId == req.UserId,ct);

    if(shoppingCart!=null)
      return TypedResults.Conflict();

    var mapper = new ShoppingCartMapper();
    var sc = mapper.ToEntity(req);

    // Agrega el nuevo carrito a la base de datos
    _dbContext.ShoppingCarts.Add(sc);
    await _dbContext.SaveChangesAsync(ct);

    var response = mapper.FromEntity(sc);

    return TypedResults.Created($"/shoppingcart/{sc.Id}", response);
  }
}
