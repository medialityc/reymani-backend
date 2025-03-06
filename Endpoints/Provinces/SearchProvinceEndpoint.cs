using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;


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
        .AsNoTracking()
        .Include(p => p.Municipalities)
        .AsQueryable();

    // Filtrado
    if (req.Ids?.Any() ?? false)
      query = query.Where(pc => req.Ids.Contains(pc.Id));

    if (req.Names?.Any() ?? false)
      query = query.Where(pc => req.Names.Any(n => pc.Name.ToLower().Contains(n.ToLower().Trim())));

    if (req.Search is not null)
    {
      var search = req.Search.ToLower().Trim();
      query = query.Where(pc => pc.Name.ToLower().Contains(search));
    }

    // Ejecución de la consulta (sin ordenamiento)
    var provinces = await query.ToListAsync(ct);

    // Ordenamiento en el cliente
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      var propertyInfo = typeof(Province).GetProperty(req.SortBy);
      if (propertyInfo != null)
      {
        provinces = req.IsDescending ?? false
            ? provinces.OrderByDescending(p => propertyInfo.GetValue(p)).ToList()
            : provinces.OrderBy(p => propertyInfo.GetValue(p)).ToList();
      }
    }

    // Paginación en el cliente
    var totalCount = provinces.Count;
    var data = provinces
        .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
        .Take(req.PageSize ?? 10)
        .ToList();

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
