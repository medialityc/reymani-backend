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
  public class GetProductByIdSystemAdminEndpoint : Endpoint<GetProductByIdRequest, Results<Ok<ProductResponse>, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetProductByIdSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/products/system-admin/{Id}");
      AllowAnonymous();
      Summary(s =>
      {
        s.Summary = "Get a product by id for system admin";
        s.Description = "Retrieves a product by id without filtering by active status.";
      });
    }

    public override async Task<Results<Ok<ProductResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetProductByIdRequest req, CancellationToken ct)
    {
      var product = await _dbContext.Products
        .Include(p => p.Business)
        .Include(p => p.Category)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

      if (product is null)
        return TypedResults.NotFound();

      var mapper = new ProductMapper();
      var responseImages = new List<string>();
      if (product.Images is not null && product.Images.Any())
      {
        foreach (var img in product.Images)
        {
          responseImages.Add(await _blobService.PresignedGetUrl(img, ct));
        }
      }
      var response = mapper.ToResponse(product, product.Business!.Name, product.Category!.Name, responseImages);

      return TypedResults.Ok(response);
    }
  }
}
