using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Products.Requests;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Products
{
  public class DeleteProductEndpoint : Endpoint<GetProductByIdRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext dbContext;

    public DeleteProductEndpoint(AppDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public override void Configure()
    {
      Delete("/products/{id}");
      Summary(s =>
      {
        s.Summary = "Delete product";
        s.Description = "Deletes a product by ID if it does not have associated OrderItem or ShoppingCartItem.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails>> ExecuteAsync(GetProductByIdRequest req, CancellationToken ct)
    {
      var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == req.Id, ct);
      if (product is null)
        return TypedResults.NotFound();

      var hasOrderItem = await dbContext.Set<OrderItem>().AsNoTracking().AnyAsync(oi => oi.ProductId == product.Id, ct);
      var hasShoppingCartItem = await dbContext.Set<ShoppingCartItem>().AsNoTracking().AnyAsync(sci => sci.ProductId == product.Id, ct);

      if (hasOrderItem || hasShoppingCartItem)
        return TypedResults.Conflict();

      dbContext.Products.Remove(product);
      await dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}