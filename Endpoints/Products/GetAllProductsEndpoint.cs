using System;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Products.Responses;
using reymani_web_api.Services.BlobServices;
using System.Linq;

namespace reymani_web_api.Endpoints.Products
{
  public class GetAllProductsEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<SimpleProductResponse>>, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetAllProductsEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/products");
      Summary(s =>
      {
        s.Summary = "Get all active products";
        s.Description = "Retrieves a list of active products.";
      });
      AllowAnonymous();
    }

    public override async Task<Results<Ok<IEnumerable<SimpleProductResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      var products = await _dbContext.Products
        .Where(p => p.IsActive)
        .OrderBy(p => p.Id)
        .AsNoTracking()
        .ToListAsync(ct);

      var mapper = new ProductMapper();
      var response = await Task.WhenAll(products.Select(async p =>
      {
        var imageUrls = new List<string>();
        if(p.Images != null && p.Images.Any())
        {
          foreach(var img in p.Images)
            imageUrls.Add(await _blobService.PresignedGetUrl(img, ct));
        }

        var ratings = await _dbContext.ProductRatings
          .Where(r => r.ProductId == p.Id)
          .ToListAsync(ct);
        var totalRatings = ratings.Count;
        var averageRating = totalRatings > 0 ? ratings.Average(r => (int)r.Rating) : 0;

        return mapper.ToSimpleProductResponse(p, imageUrls, totalRatings, (decimal)averageRating);
      }));

      return TypedResults.Ok(response.AsEnumerable());
    }
  }
}
