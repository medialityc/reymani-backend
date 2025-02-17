using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ProductCategories.Responses;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.Mappers;
using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Endpoints.ProductCategories
{
  public class GetAllProductCategoriesSystemAdminEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<ProductCategoryResponse>>, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetAllProductCategoriesSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/product-categories/system-admin");
      Summary(s =>
      {
        s.Summary = "Get all product categories for system admin";
        s.Description = "Retrieves a list of all product categories, without filtering by active status.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok<IEnumerable<ProductCategoryResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      var categories = _dbContext.ProductCategories
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
