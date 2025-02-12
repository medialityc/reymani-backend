using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ProductCategories.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.ProductCategories
{
  public class GetAllProductCategoriesEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<ProductCategoryResponse>>, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetAllProductCategoriesEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/productcategories");
      Summary(s =>
      {
        s.Summary = "Get all active product categories";
        s.Description = "Retrieves a list of active product categories.";
      });
    }

    public override async Task<Results<Ok<IEnumerable<ProductCategoryResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      var categories = _dbContext.ProductCategories
        .Where(pc => pc.IsActive)
        .OrderBy(pc => pc.Id)
        .AsEnumerable();

      var response = await Task.WhenAll(categories.Select(async pc => new ProductCategoryResponse
      {
        Id = pc.Id,
        Name = pc.Name,
        Logo = pc.Logo != null ? await _blobService.PresignedGetUrl(pc.Logo, ct) : null,
        IsActive = pc.IsActive
      }));

      return TypedResults.Ok(response.AsEnumerable());
    }
  }
}
