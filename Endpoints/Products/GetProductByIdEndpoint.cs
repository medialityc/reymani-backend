using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Products.Requests;
using reymani_web_api.Endpoints.Products.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Products
{
  public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, Results<Ok<ProductResponse>, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetProductByIdEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/products/{Id}");
      AllowAnonymous();
      Summary(s =>
      {
        s.Summary = "Get an active product by id";
        s.Description = "Retrieves an active product by id.";
      });
    }

    public override async Task<Results<Ok<ProductResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetProductByIdRequest req, CancellationToken ct)
    {
      var product = await _dbContext.Products
        .Where(p => p.IsActive)
        .Include(p => p.Business)
        .Include(p => p.Category)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

      if (product is null)
        return TypedResults.NotFound();

      // Obtener valoraciones del producto
      var ratings = await _dbContext.ProductRatings
        .Where(r => r.ProductId == req.Id)
        .ToListAsync(ct);
      var totalRatings = ratings.Count;
      var averageRating = totalRatings > 0 ? ratings.Average(r => (int)r.Rating) : 0;

      var mapper = new ProductMapper();
      var responseImages = new List<string>();
      if (product.Images is not null && product.Images.Any())
      {
        foreach (var img in product.Images)
        {
          responseImages.Add(await _blobService.PresignedGetUrl(img, ct));
        }
      }
      var response = mapper.ToResponse(product, product.Business!.Name, product.Category!.Name, responseImages, totalRatings, (decimal)averageRating);

      return TypedResults.Ok(response);
    }
  }
}
