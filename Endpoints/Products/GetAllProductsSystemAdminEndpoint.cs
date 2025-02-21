using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Products.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Products;

public class GetAllProductsSystemAdminEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<SimpleProductResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetAllProductsSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/products/system-admin");
    Summary(s =>
    {
      s.Summary = "Get all products for system admin";
      s.Description = "Retrieves a list of all products for system admin.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<IEnumerable<SimpleProductResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var products = await _dbContext.Products
      .OrderBy(p => p.Id)
      .AsNoTracking()
      .ToListAsync(ct);

    var productIds = products.Select(p => p.Id).ToList();
    var ratingsList = await _dbContext.ProductRatings
      .Where(r => productIds.Contains(r.ProductId))
      .AsNoTracking()
      .ToListAsync(ct);
    var ratingsByProduct = ratingsList.GroupBy(r => r.ProductId)
      .ToDictionary(g => g.Key, g => g.ToList());

    var mapper = new ProductMapper();
    var responses = new List<SimpleProductResponse>();
    foreach (var p in products)
    {
      var imageUrls = new List<string>();
      if (p.Images != null && p.Images.Any())
      {
        foreach (var img in p.Images)
          imageUrls.Add(await _blobService.PresignedGetUrl(img, ct));
      }

      ratingsByProduct.TryGetValue(p.Id, out var ratings);
      ratings ??= new List<ProductRating>();
      var totalRatings = ratings.Count;
      var averageRating = totalRatings > 0 ? (decimal)ratings.Average(r => (int)r.Rating) : 0;

      responses.Add(mapper.ToSimpleProductResponse(p, imageUrls, totalRatings, averageRating));
    }

    return TypedResults.Ok(responses.AsEnumerable());
  }
}