using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.ShoppingCarts.Responses;

namespace reymani_web_api.Endpoints.ShoppingCarts;

public class InitShoppingCartEndpoint : EndpointWithoutRequest<Results<Created<ShoppingCartResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  public InitShoppingCartEndpoint(AppDbContext dbContext)
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
    Roles("Customer");

  }

  public override async Task<Results<Created<ShoppingCartResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync( CancellationToken ct)
  {
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      return TypedResults.Unauthorized();

    var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(p => p.UserId == userId,ct);

    var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userId, ct);

    var userAddress = await _dbContext.UserAddresses.FirstOrDefaultAsync(p => p.UserId == userId);

    if(shoppingCart!=null)
      return TypedResults.Conflict();

    if(user==null || userAddress == null)
      return TypedResults.Conflict();

    var mapper = new ShoppingCartMapper();
    var sc = mapper.ToEntity(user.Id,userAddress.Id);

    // Agrega el nuevo carrito a la base de datos
    _dbContext.ShoppingCarts.Add(sc);
    await _dbContext.SaveChangesAsync(ct);

    var response = mapper.FromEntity(sc);

    return TypedResults.Created($"/shoppingcart/{sc.Id}", response);
  }
}
