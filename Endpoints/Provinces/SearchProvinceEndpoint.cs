using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.Users.Responses;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Provinces;

public class SearchProvincesEndpoint : Endpoint<SearchProvincesRequest, Results<Ok<PaginatedResponse<ProvinceResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchProvincesEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/provinces/search");
    Summary(s =>
    {
      s.Summary = "Search provinces";
      s.Description = "Search for provinces by name or ID with filtering, sorting, and pagination.";
    });
    AllowAnonymous();
  }

  public override async Task<Results<Ok<PaginatedResponse<ProvinceResponse>>, ProblemDetails>> ExecuteAsync(SearchProvincesRequest req, CancellationToken ct)
  {
    var query = _dbContext.Provinces
      .Include(p=> p.Municipalities)
      .AsQueryable();

    // Filtrado
    if (!string.IsNullOrEmpty(req.Name))
    {
      query = query.Where(p => p.Name.Contains(req.Name));
    }

    if (req.Id.HasValue)
    {
      query = query.Where(p => p.Id == req.Id.Value);
    }

    // Ordenamiento
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      var propertyInfo = typeof(Province).GetProperty(req.SortBy);
      if (propertyInfo != null)
      {
        query = req.IsDescending ?? false
        ? query.OrderByDescending(u => propertyInfo.GetValue(u))
            : query.OrderBy(u => propertyInfo.GetValue(u));
      }
    }

    // Paginación
    var totalCount = await query.CountAsync(ct);
    var data = await query
           .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
           .Take(req.PageSize ?? 10)
           .ToListAsync(ct);

    // Mapeo de respuesta
    var mapper = new ProvinceMapper();
    var responseData = data.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(new PaginatedResponse<ProvinceResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
