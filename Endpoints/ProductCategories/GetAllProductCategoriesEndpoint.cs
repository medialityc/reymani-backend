using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
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
      Get("/product-categories");
      Summary(s =>
      {
        s.Summary = "Get all active product categories";
        s.Description = "Retrieves a list of active product categories.";
      });
      AllowAnonymous();
    }

    public override async Task<Results<Ok<IEnumerable<ProductCategoryResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      var categories = _dbContext.ProductCategories
        .Where(pc => pc.IsActive)
        .OrderBy(pc => pc.Id)
        .AsNoTracking()
        .AsEnumerable();

      var mapper = new ProductCategoryMapper();
      var response = await Task.WhenAll(categories.Select(async pc =>
      {
        var res = mapper.FromEntity(pc);
        res.Logo = !string.IsNullOrEmpty(pc.Logo)
            ? await _blobService.PresignedGetUrl(pc.Logo, ct)
            : null;
        return res;
      }));

      return TypedResults.Ok(response.AsEnumerable());
    }
  }
}