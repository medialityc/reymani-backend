using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Municipalities.Responses;


namespace reymani_web_api.Endpoints.Municipalities;

public class SearchMunicipalitiesEndpoint : Endpoint<SearchMunicipalityRequest, Results<Ok<PaginatedResponse<MunicipalityWithNameProvinceResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchMunicipalitiesEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/municipalities/search");
    Summary(s =>
    {
      s.Summary = "Search municipalities";
      s.Description = "Search for municipalities by name or ID with filtering, sorting, and pagination.";
    });
    AllowAnonymous();
  }

  public override async Task<Results<Ok<PaginatedResponse<MunicipalityWithNameProvinceResponse>>, ProblemDetails>> ExecuteAsync(SearchMunicipalityRequest req, CancellationToken ct)
  {
    var query = _dbContext.Municipalities
        .AsNoTracking()
        .Include(p => p.Province)
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

    // Ordenamiento en la base de datos
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      query = req.IsDescending ?? false
          ? query.OrderByDescending(pc => EF.Property<object>(pc, req.SortBy)) // Ordenamiento dinámico
          : query.OrderBy(pc => EF.Property<object>(pc, req.SortBy));
    }

    // Conteo total (sin paginación)
    var totalCount = await query.CountAsync(ct);

    // Paginación
    var data = await query
        .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
        .Take(req.PageSize ?? 10)
        .ToListAsync(ct);

    // Mapeo de respuesta
    var mapper = new MunicipalitiesMapper();
    var responseData = data.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(new PaginatedResponse<MunicipalityWithNameProvinceResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
