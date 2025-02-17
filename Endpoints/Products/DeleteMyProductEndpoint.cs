using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Products.Requests;

using ReymaniWebApi.Data.Models;


namespace reymani_web_api.Endpoints.Products;

public class DeleteMyProductEndpoint : Endpoint<GetProductByIdRequest, Results<Ok, NotFound, Conflict, ProblemDetails, UnauthorizedHttpResult>>
{
  private readonly AppDbContext dbContext;

  public DeleteMyProductEndpoint(AppDbContext dbContext)
  {
    this.dbContext = dbContext;
  }

  public override void Configure()
  {
    Delete("/products/my/{id}");
    Summary(s =>
    {
      s.Summary = "Delete my product";
      s.Description = "Deletes a product by ID if it belongs to the business associated with the current user.";
    });
    Roles("BusinessAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails, UnauthorizedHttpResult>> ExecuteAsync(GetProductByIdRequest req, CancellationToken ct)
  {
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
      return TypedResults.Unauthorized();
    }

    var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, ct);
    if (user == null || user.Role != Data.Models.UserRole.BusinessAdmin)
    {
      return TypedResults.Unauthorized();
    }

    var business = await dbContext.Businesses.FirstOrDefaultAsync(x => x.UserId == userId, ct);
    if (business == null)
    {
      return TypedResults.Unauthorized();
    }

    var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == req.Id, ct);
    if (product is null || product.BusinessId != business.Id)
    {
      return TypedResults.NotFound();
    }

    var hasOrderItem = await dbContext.Set<OrderItem>().AsNoTracking().AnyAsync(oi => oi.ProductId == product.Id, ct);
    var hasShoppingCartItem = await dbContext.Set<ShoppingCartItem>().AsNoTracking().AnyAsync(sci => sci.ProductId == product.Id, ct);

    if (hasOrderItem || hasShoppingCartItem)
    {
      return TypedResults.Conflict();
    }

    dbContext.Products.Remove(product);
    await dbContext.SaveChangesAsync(ct);
    return TypedResults.Ok();
  }
}
