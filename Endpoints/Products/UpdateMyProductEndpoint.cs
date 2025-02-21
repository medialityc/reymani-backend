using System;
using System.Linq;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Products.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Products
{
  public class UpdateMyProductEndpoint : Endpoint<UpdateProductRequest, Results<Ok, NotFound, Conflict, ProblemDetails, UnauthorizedHttpResult>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateMyProductEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Put("/products/my/{id}");
      Summary(s =>
      {
        s.Summary = "Update my product";
        s.Description = "Updates details of a product for the authenticated BusinessAdmin's business.";
      });
      Roles("BusinessAdmin");
      AllowFormData();
    }

    public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails, UnauthorizedHttpResult>> ExecuteAsync(UpdateProductRequest req, CancellationToken ct)
    {
      // Obtener id de usuario desde el JWT.
      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      {
        return TypedResults.Unauthorized();
      }

      // Verificar que el usuario exista y sea BusinessAdmin.
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId, ct);
      if (user == null || user.Role != Data.Models.UserRole.BusinessAdmin)
      {
        return TypedResults.Unauthorized();
      }

      // Obtener el negocio asociado al administrador.
      var business = await _dbContext.Businesses.FirstOrDefaultAsync(x => x.UserId == userId, ct);
      if (business == null)
      {
        return TypedResults.NotFound();
      }

      // Validar que no exista otro producto con el mismo nombre en el negocio.
      var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Name == req.Name && x.BusinessId == business.Id, ct);
      if (existingProduct != null && existingProduct.Id != req.Id)
      {
        return TypedResults.Conflict();
      }

      // Recuperar el producto perteneciente al negocio.
      var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == req.Id && x.BusinessId == business.Id, ct);
      if (product is null)
      {
        return TypedResults.NotFound();
      }

      // Actualizar el producto usando el mapper.
      var mapper = new ProductMapper();
      product = mapper.UpdateEntity(req, product);

      // Siempre borrar las imágenes existentes.
      product.Images!.Clear();
      // Agregar nuevas imágenes solo si se proporcionan.
      if (req.Images != null && req.Images.Any())
      {
        foreach (var image in req.Images)
        {
          var fileCode = Guid.NewGuid().ToString();
          string imagePath = await _blobService.UploadObject(image, fileCode, ct);
          product.Images.Add(imagePath);
        }
      }

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}