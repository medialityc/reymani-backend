using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Products.Requests;
using reymani_web_api.Endpoints.Products.Responses;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Products
{
  public class SearchProductsEndpoint(AppDbContext dbContext, IBlobService blobService)
      : Endpoint<SearchProductsRequest, Results<Ok<PaginatedResponse<ProductResponse>>, ProblemDetails>>
  {
    public override void Configure()
    {
      Get("/products/search");
      Summary(s =>
      {
        s.Summary = "Search for active products";
        s.Description = "Searches for active products based on the specified criteria.";
      });
      AllowAnonymous();
    }

    public override async Task<Results<Ok<PaginatedResponse<ProductResponse>>, ProblemDetails>>
        ExecuteAsync(SearchProductsRequest req, CancellationToken ct)
    {
      // Inicializar la query e incluir Business y Category.
      var query = dbContext.Products
          .AsNoTracking()
          .Include(p => p.Business)
          .Include(p => p.Category)
          .AsQueryable();

      // Considerar solo productos activos.
      query = query.Where(p => p.IsActive);

      if (req.Ids?.Any() ?? false)
        query = query.Where(p => req.Ids.Contains(p.Id));

      if (req.Names?.Any() ?? false)
        query = query.Where(p => req.Names.Any(n =>
            p.Name.ToLower().Contains(n.ToLower().Trim())));

      if (req.Description?.Any() ?? false)
        query = query.Where(p => p.Description != null && req.Description.Any(d =>
            p.Description.ToLower().Contains(d.ToLower().Trim())));

      if (req.BusinessId.HasValue)
        query = query.Where(p => p.BusinessId == req.BusinessId.Value);

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