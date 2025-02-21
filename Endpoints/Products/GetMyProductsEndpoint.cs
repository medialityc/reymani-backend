using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Products.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Products
{
  public class GetMyProductsEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<ProductResponse>>, ProblemDetails, UnauthorizedHttpResult>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetMyProductsEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/products/my");
      Summary(s =>
      {
        s.Summary = "Get my products";
        s.Description = "Retrieves a list of products for the business associated with the current user.";
      });
      Roles("BusinessAdmin");
    }

    public override async Task<Results<Ok<IEnumerable<ProductResponse>>, ProblemDetails, UnauthorizedHttpResult>> ExecuteAsync(CancellationToken ct)
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

      var business = await _dbContext.Businesses.FirstOrDefaultAsync(x => x.UserId == userId, ct);
      if (business == null)
      {
        AddError("Negocio no encontrado para el administrador.");
      }

      ThrowIfAnyErrors();

      // Obtener productos del negocio
      var products = await _dbContext.Products
          .Where(x => x.BusinessId == business!.Id)
          .Include(p => p.Business)
          .Include(p => p.Category)
          .ToListAsync(ct);

      var mapper = new ProductMapper();
      var responses = new List<ProductResponse>();

      foreach (var product in products)
      {
        // Obtener URLs presignadas para cada imagen
        var responseImages = new List<string>();
        if (product.Images is not null && product.Images.Any())
        {
          foreach (var img in product.Images)
          {
            var url = await _blobService.PresignedGetUrl(img, ct);
            responseImages.Add(url);
          }
        }

        var ratings = await _dbContext.ProductRatings
          .Where(r => r.ProductId == product.Id)
          .ToListAsync(ct);
        var totalRatings = ratings.Count;
        var averageRating = totalRatings > 0 ? ratings.Average(r => (int)r.Rating) : 0;

        // Consultar categoría del producto
        var category = await _dbContext.ProductCategories.FirstOrDefaultAsync(x => x.Id == product.CategoryId, ct);

        var response = mapper.ToResponse(product, product.Business!.Name, product.Category!.Name, responseImages, totalRatings, (decimal)averageRating);
        responses.Add(response);
      }

      return TypedResults.Ok(responses.AsEnumerable());
    }
  }
}