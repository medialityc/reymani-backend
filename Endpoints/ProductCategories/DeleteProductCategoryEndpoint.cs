using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ProductCategories.Requests;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ProductCategories
{
  public class DeleteProductCategoryEndpoint : Endpoint<GetProductCategoryByIdRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext dbContext;

    public DeleteProductCategoryEndpoint(AppDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public override void Configure()
    {
      Delete("/product-categories/{id}");
      Summary(s =>
      {
        s.Summary = "Delete product category";
        s.Description = "Deletes an existing product category by ID if not in use.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails>> ExecuteAsync(GetProductCategoryByIdRequest req, CancellationToken ct)
    {
      var category = await dbContext.Set<ProductCategory>().FindAsync(new object[] { req.Id }, ct);
      if (category is null)
        return TypedResults.NotFound();

      bool inUse = await dbContext.Set<Product>().AnyAsync(p => p.CategoryId == req.Id, ct);
      if (inUse)
        return TypedResults.Conflict();

      dbContext.Set<ProductCategory>().Remove(category);
      await dbContext.SaveChangesAsync(ct);

      return TypedResults.Ok();
    }
  }
}