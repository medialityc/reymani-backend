using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;

using reymani_web_api.Endpoints.ProductCategories.Requests;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;
using reymani_web_api.Endpoints.Mappers;

namespace reymani_web_api.Endpoints.ProductCategories
{
  public class CreateProductCategoryEndpoint : Endpoint<CreateProductCategoryRequest, Results<Created, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public CreateProductCategoryEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Post("/product-categories");
      Summary(s =>
      {
        s.Summary = "Create product category";
        s.Description = "Creates a new product category by verifying its uniqueness and uploading its logo via Minio.";
      });
      Roles("SystemAdmin");
      AllowFormData();
    }

    public override async Task<Results<Created, Conflict, ProblemDetails>> ExecuteAsync(CreateProductCategoryRequest req, CancellationToken ct)
    {
      var existingProductCategory = await _dbContext.ProductCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Name == req.Name.Trim(), ct);
      if (existingProductCategory != null)
      {
        return TypedResults.Conflict();
      }

      var mapper = new ProductCategoryMapper();
      var productCategory = mapper.ToEntity(req);

      if (req.Logo != null)
      {
        string fileCode = Guid.NewGuid().ToString();
        string objectPath = await _blobService.UploadObject(req.Logo, fileCode, ct);
        productCategory.Logo = objectPath;
      }

      await _dbContext.ProductCategories.AddAsync(productCategory, ct);
      await _dbContext.SaveChangesAsync(ct);

      return TypedResults.Created();
    }
  }
}
