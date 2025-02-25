using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Products.Requests;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Products
{
  public class DeleteProductEndpoint : Endpoint<GetProductByIdRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteProductEndpoint(AppDbContext _dbContext, IBlobService _blobService)
    {
      this._dbContext = _dbContext;
      this._blobService = _blobService;
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
      var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == req.Id, ct);
      if (product is null)
        return TypedResults.NotFound();

      var hasOrderItem = await _dbContext.Set<OrderItem>().AsNoTracking().AnyAsync(oi => oi.ProductId == product.Id, ct);
      var hasShoppingCartItem = await _dbContext.Set<ShoppingCartItem>().AsNoTracking().AnyAsync(sci => sci.ProductId == product.Id, ct);

      if (hasOrderItem || hasShoppingCartItem)
        return TypedResults.Conflict();

      _dbContext.Products.Remove(product);
      await _dbContext.SaveChangesAsync(ct);

      if (product.Images != null)
      {
        foreach (var image in product.Images)
        {
          await _blobService.DeleteObject(image, ct);
        }
      }

      return TypedResults.Ok();
    }
  }
}