using System;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.ProductCategories.Responses;
using reymani_web_api.Endpoints.ProductCategories.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.ProductCategories
{
  public class GetProductCategoryByIdSystemAdminEndpoint : Endpoint<GetProductCategoryByIdRequest, Results<Ok<ProductCategoryResponse>, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetProductCategoryByIdSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/product-categories/system-admin/{Id}");
      Summary(s =>
      {
        s.Summary = "Get any product category by Id for system admin";
        s.Description = "Retrieves a single product category by Id, including inactive ones.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok<ProductCategoryResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetProductCategoryByIdRequest req, CancellationToken ct)
    {
      // Buscar la categoría por Id sin filtrar por IsActive
      var pc = await _dbContext.ProductCategories.FirstOrDefaultAsync(p => p.Id == req.Id, ct);
      if (pc == null)
        return TypedResults.NotFound();

      var response = new ProductCategoryResponse
      {
        Id = pc.Id,
        Name = pc.Name,
        Logo = pc.Logo != null ? await _blobService.PresignedGetUrl(pc.Logo, ct) : null,
        IsActive = pc.IsActive
      };

      return TypedResults.Ok(response);
    }
  }
}
