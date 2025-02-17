using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Products.Requests;
using reymani_web_api.Endpoints.Products.Responses;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Commons.Responses;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Products
{
  public class SearchMyProductsEndpoint(AppDbContext dbContext, IBlobService blobService)
      : Endpoint<SearchProductsSystemAdminRequest, Results<Ok<PaginatedResponse<ProductResponse>>, ProblemDetails, UnauthorizedHttpResult, NotFound>>
  {
    public override void Configure()
    {
      Get("/products/my/search");
      Summary(s =>
      {
        s.Summary = "Search for products";
        s.Description = "Searches for products based on the specified criteria.";
      });
      Roles("BusinessAdmin");
    }

    public override async Task<Results<Ok<PaginatedResponse<ProductResponse>>, ProblemDetails, UnauthorizedHttpResult, NotFound>>
        ExecuteAsync(SearchProductsSystemAdminRequest req, CancellationToken ct)
    {
      // Obtener negocio del usuario autenticado
      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        return TypedResults.Unauthorized();

      var business = await dbContext.Businesses.FirstOrDefaultAsync(x => x.UserId == userId, ct);
      if (business == null)
        return TypedResults.NotFound();

      // Inicializar la query y filtrar por el negocio autenticado.
      var query = dbContext.Products
          .AsNoTracking()
          .Include(p => p.Business)
          .Include(p => p.Category)
          .Where(p => p.BusinessId == business.Id) // nuevo filtro para el negocio
          .AsQueryable();

      if (req.Ids?.Any() ?? false)
        query = query.Where(p => req.Ids.Contains(p.Id));

      if (req.Names?.Any() ?? false)
        query = query.Where(p => req.Names.Any(n =>
            p.Name.ToLower().Contains(n.ToLower().Trim())));

      if (req.Description?.Any() ?? false)
        query = query.Where(p => p.Description != null && req.Description.Any(d =>
            p.Description.ToLower().Contains(d.ToLower().Trim())));

      // Ignorar filtro de BusinessId desde la request ya que se usa el del usuario autenticado.
      if (req.CategoryId.HasValue)
        query = query.Where(p => p.CategoryId == req.CategoryId.Value);

      if (req.Capacity.HasValue)
        query = query.Where(p => p.Capacity >= req.Capacity.Value);

      if (req.PriceMin.HasValue)
        query = query.Where(p => p.Price >= req.PriceMin.Value);

      if (req.PriceMax.HasValue)
        query = query.Where(p => p.Price <= req.PriceMax.Value);

      if (req.HasDiscount.HasValue && req.HasDiscount.Value)
        query = query.Where(p => p.DiscountPrice > 0);

      if (req.IsAvailable.HasValue)
        query = query.Where(p => p.IsAvailable == req.IsAvailable.Value);

      if (req.IsActive.HasValue)
        query = query.Where(p => p.IsActive == req.IsActive.Value);

      if (req.Search is not null)
      {
        var search = req.Search.ToLower().Trim();
        query = query.Where(p =>
            p.Name.ToLower().Contains(search) ||
            (p.Description != null && p.Description.ToLower().Contains(search))
        );
      }

      // Ordenamiento
      if (!string.IsNullOrEmpty(req.SortBy))
      {
        var propertyInfo = typeof(Product).GetProperty(req.SortBy);
        if (propertyInfo != null)
        {
          query = req.IsDescending ?? false
              ? query.OrderByDescending(p => EF.Property<object>(p, req.SortBy))
              : query.OrderBy(p => EF.Property<object>(p, req.SortBy));
        }
      }

      // Paginación
      var totalCount = await query.CountAsync(ct);
      var data = await query
          .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
          .Take(req.PageSize ?? 10)
          .ToListAsync(ct);

      var mapper = new ProductMapper();
      var responseData = new List<ProductResponse>();
      foreach (var p in data)
      {
        // Obtener imágenes presignadas
        var responseImages = new List<string>();
        if (p.Images != null && p.Images.Any())
        {
          foreach (var img in p.Images)
            responseImages.Add(await blobService.PresignedGetUrl(img, ct));
        }

        // Obtener información de ratings
        var ratings = await dbContext.ProductRatings
          .Where(r => r.ProductId == p.Id)
          .ToListAsync(ct);
        var totalRatings = ratings.Count;
        var averageRating = totalRatings > 0 ? (decimal)ratings.Average(r => (int)r.Rating) : 0m;

        // Usar valores de Business y Category
        var businessName = p.Business?.Name ?? string.Empty;
        var categoryName = p.Category?.Name ?? string.Empty;

        responseData.Add(mapper.ToResponse(p, businessName, categoryName, responseImages, totalRatings, averageRating));
      }

      return TypedResults.Ok(new PaginatedResponse<ProductResponse>
      {
        Data = responseData,
        Page = req.Page ?? 1,
        PageSize = req.PageSize ?? 10,
        TotalCount = totalCount
      });
    }
  }
}
