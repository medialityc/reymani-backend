using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ProductCategories.Requests;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Utils.Mappers;

namespace reymani_web_api.Endpoints.ProductCategories
{
  public class UpdateProductCategoryEndpoint : Endpoint<UpdateProductCategoryRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateProductCategoryEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Put("/product-categories/{id}");
      Summary(s =>
      {
        s.Summary = "Update product category";
        s.Description = "Updates details of an existing product category.";
      });
      Roles("SystemAdmin");
      AllowFormData();
    }

    public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails>> ExecuteAsync(UpdateProductCategoryRequest req, CancellationToken ct)
    {
      // Verificar duplicado por nombre
      var existingCategory = await _dbContext.ProductCategories.FirstOrDefaultAsync(x => x.Name == req.Name, ct);
      if (existingCategory != null && existingCategory.Id != req.Id)
      {
        return TypedResults.Conflict();
      }

      // Buscar la categoria por Id
      var category = await _dbContext.ProductCategories.FindAsync(new object?[] { req.Id }, ct);
      if (category is null)
        return TypedResults.NotFound();

      var mapper = new ProductCategoryMapper();
      // Actualizar entidad con los datos de la request
      category = mapper.ToEntity(req, category);

      if (req.Logo != null)
      {
        string fileCode = Guid.NewGuid().ToString();
        string objectPath = await _blobService.UploadObject(req.Logo, fileCode, ct);
        category.Logo = objectPath;
      }

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}
