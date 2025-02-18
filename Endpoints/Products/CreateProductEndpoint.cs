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
  public class CreateProductEndpoint : Endpoint<CreateProductRequest, Results<Created<ProductResponse>, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public CreateProductEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Post("/products");
      Summary(s =>
      {
        s.Summary = "Create product";
        s.Description = "Creates a new product.";
      });
      AllowFormData();
      Roles("SystemAdmin");
    }

    public override async Task<Results<Created<ProductResponse>, Conflict, ProblemDetails>> ExecuteAsync(CreateProductRequest req, CancellationToken ct)
    {
      // Obtener negocio 
      var business = await _dbContext.Businesses.FirstOrDefaultAsync(x => x.Id == req.BusinessId, ct);
      if (business == null)
      {
        AddError(req => req.BusinessId, "Negocio no encontrado.");
      }

      // Validar categoría
      var category = await _dbContext.ProductCategories.FirstOrDefaultAsync(x => x.Id == req.CategoryId, ct);
      if (category == null)
      {
        AddError(req => req.CategoryId, "Categoría no encontrada.");
      }

      ThrowIfAnyErrors();

      // Comprobar existencia de un producto con el mismo nombre para este negocio
      var existingProduct = await _dbContext.Products.AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Name == req.Name && x.BusinessId == business!.Id, ct);
      if (existingProduct != null)
      {
        return TypedResults.Conflict();
      }

      ThrowIfAnyErrors();

      // Reemplazar mapeo manual por el mapper
      var mapper = new ProductMapper();
      var product = mapper.ToEntity(req, business!.Id);

      // Subir imágenes si las hay
      if (req.Images is not null && req.Images.Any())
      {
        foreach (var image in req.Images)
        {
          var fileCode = Guid.NewGuid().ToString();
          string objectPath = await _blobService.UploadObject(image, fileCode, ct);
          product.Images!.Add(objectPath);
        }
      }

      _dbContext.Products.Add(product);
      business.Products!.Add(product);
      await _dbContext.SaveChangesAsync(ct);

      // Preparar respuesta convirtiendo las rutas de las imágenes a URLs presignadas
      var responseImages = new List<string>();
      if (product.Images is not null && product.Images.Any())
      {
        foreach (var img in product.Images)
        {
          var url = await _blobService.PresignedGetUrl(img, ct);
          responseImages.Add(url);
        }
      }

      var response = mapper.ToResponse(product, business.Name, category!.Name, responseImages, 0, 0);

      return TypedResults.Created($"/products/{product.Id}", response);
    }
  }
}