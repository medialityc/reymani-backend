using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Products.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Products
{
  public class UpdateProductEndpoint : Endpoint<UpdateProductRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateProductEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Put("/products/{id}");
      Summary(s =>
      {
        s.Summary = "Update product";
        s.Description = "Updates details of an existing product.";
      });
      Roles("SystemAdmin");
      AllowFormData();
    }

    public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails>> ExecuteAsync(UpdateProductRequest req, CancellationToken ct)
    {
      // Primero, recuperar el producto para obtener su BusinessId.
      var product = await _dbContext.Products.FindAsync(new object?[] { req.Id }, ct);
      if (product is null)
        return TypedResults.NotFound();

      // Validar que no exista otro producto en el mismo negocio con el mismo nombre.
      var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(
        x => x.Name == req.Name && x.BusinessId == product.BusinessId && x.Id != req.Id,
        ct);
      if (existingProduct != null)
      {
        return TypedResults.Conflict();
      }

      // Actualizar el producto usando el mapper.
      var mapper = new ProductMapper();
      product = mapper.UpdateEntity(req, product);

      // Siempre borrar las imágenes existentes.
      product.Images!.Clear();
      // Agregar únicamente si se envía alguna imagen.
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