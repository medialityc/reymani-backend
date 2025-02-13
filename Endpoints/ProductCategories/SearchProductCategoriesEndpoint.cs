using System;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.ProductCategories.Requests;
using reymani_web_api.Endpoints.Commons.Responses;
using ReymaniWebApi.Data.Models;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.ProductCategories.Responses;

namespace reymani_web_api.Endpoints.ProductCategories
{
  public class SearchProductCategoriesEndpoint(AppDbContext dbContext, IBlobService blobService)
      : Endpoint<SearchProductCategoriesRequest, Results<Ok<PaginatedResponse<ProductCategoryResponse>>, ProblemDetails>>
  {
    public override void Configure()
    {
      Get("/product-categories/search");
      Summary(s =>
      {
        s.Summary = "Search product categories";
        s.Description = "Searches for product categories based on the specified criteria.";
      });
      AllowAnonymous();
    }

    public override async Task<Results<Ok<PaginatedResponse<ProductCategoryResponse>>, ProblemDetails>>
        ExecuteAsync(SearchProductCategoriesRequest req, CancellationToken ct)
    {
      var query = dbContext.ProductCategories.AsNoTracking().AsQueryable();

      // Filtros
      if (req.Ids?.Any() ?? false)
        query = query.Where(pc => req.Ids.Contains(pc.Id));

      if (req.Names?.Any() ?? false)
        query = query.Where(pc => req.Names.Any(n => pc.Name.ToLower().Contains(n.ToLower().Trim())));

      if (req.Search is not null)
      {
        var search = req.Search.ToLower().Trim();
        query = query.Where(pc => pc.Name.ToLower().Contains(search));
      }

      // Ejecución de la consulta
      var categories = (await query.ToListAsync(ct)).AsEnumerable();

      // Ordenamiento
      if (!string.IsNullOrEmpty(req.SortBy))
      {
        var propertyInfo = typeof(ProductCategory).GetProperty(req.SortBy);
        if (propertyInfo != null)
        {
          categories = req.IsDescending ?? false
              ? categories.OrderByDescending(pc => propertyInfo.GetValue(pc))
              : categories.OrderBy(pc => propertyInfo.GetValue(pc));
        }
      }

      // Paginación
      var totalCount = categories.Count();
      var data = categories.Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
                           .Take(req.PageSize ?? 10);

      // Mapeo a ProductCategoryResponse
      var responseData = await Task.WhenAll(data.Select(async pc => new ProductCategoryResponse
      {
        Id = pc.Id,
        Name = pc.Name,
        Logo = !string.IsNullOrEmpty(pc.Logo)
                 ? await blobService.PresignedGetUrl(pc.Logo, ct)
                 : null,
        IsActive = pc.IsActive
      }));

      return TypedResults.Ok(new PaginatedResponse<ProductCategoryResponse>
      {
        Data = responseData,
        Page = req.Page ?? 1,
        PageSize = req.PageSize ?? 10,
        TotalCount = totalCount
      });
    }
  }

}
