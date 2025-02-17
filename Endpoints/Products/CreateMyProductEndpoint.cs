using System;

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
  public class CreateMyProductEndpoint : Endpoint<CreateMyProductRequest, Results<Created<ProductResponse>, Conflict, ProblemDetails, UnauthorizedHttpResult>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public CreateMyProductEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Post("/products/my");
      Summary(s =>
      {
        s.Summary = "Create my product";
        s.Description = "Creates a new product for the business associated with the current user.";
      });
      AllowFormData();
      Roles("BusinessAdmin");
    }

    public override async Task<Results<Created<ProductResponse>, Conflict, ProblemDetails, UnauthorizedHttpResult>> ExecuteAsync(CreateMyProductRequest req, CancellationToken ct)
    {
      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      {
        return TypedResults.Unauthorized();
      }

      // Utilizar el id obtenido de claims
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, ct);
      if (user == null || user.Role != Data.Models.UserRole.BusinessAdmin)
      {
        AddError("Usuario no encontrado o no es un administrador de negocio.");
      }

      // Utilizar el id obtenido de claims para obtener el negocio
      var business = await _dbContext.Businesses.FirstOrDefaultAsync(x => x.UserId == userId, ct);
      if (business == null)
      {
        AddError("Negocio no encontrado para el administrador.");
      }

      // Validar categoría
      var category = await _dbContext.ProductCategories.FirstOrDefaultAsync(x => x.Id == req.CategoryId, ct);
      if (category == null)
      {
        AddError(req => req.CategoryId, "Categoría no encontrada.");
      }

      ThrowIfAnyErrors();

      // Verificar existencia de un producto con el mismo nombre para este negocio
      var existingProduct = await _dbContext.Products.AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Name == req.Name && x.BusinessId == business!.Id, ct);
      if (existingProduct != null)
      {
        return TypedResults.Conflict();
      }

      ThrowIfAnyErrors();

      // Utilizar el nuevo método del mapper para convertir el request a entidad
      var mapper = new ProductMapper();
      var product = mapper.ToEntity(req, business!.Id);

      // Subir imágenes si están presentes
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

      // Convertir rutas de imágenes a URLs presignadas para la respuesta
      var responseImages = new List<string>();
      if (product.Images is not null && product.Images.Any())
      {
        foreach (var img in product.Images)
        {
          var url = await _blobService.PresignedGetUrl(img, ct);
          responseImages.Add(url);
        }
      }

      var response = mapper.ToResponse(product, business.Name, category!.Name, responseImages);

      return TypedResults.Created($"/products/{product.Id}", response);
    }
  }
}
