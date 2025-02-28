using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.ShippingCost.Request;
using reymani_web_api.Endpoints.ShippingsCost.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ShippingsCost;

public class SearchShippingCostEndpoint : Endpoint<SearchShippingCostRequest, Results<Ok<PaginatedResponse<ShippingCostResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchShippingCostEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/shippingcost/search");
    Summary(s =>
    {
      s.Summary = "Search shipping cost";
      s.Description = "Search for shipping cost by cost max and min or ID with filtering, sorting, and pagination.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<PaginatedResponse<ShippingCostResponse>>, ProblemDetails>> ExecuteAsync(SearchShippingCostRequest req, CancellationToken ct)
  {
    var query = _dbContext.ShippingCosts
        .AsNoTracking()
        .Include(p => p.Municipality)
        .Include(p => p.VehicleType)
        .AsQueryable();

    // Filtrado por IDs
    if (req.Ids?.Any() ?? false)
      query = query.Where(pc => req.Ids.Contains(pc.Id));

    // Filtrado por IDs de Municipios
    if (req.MunicipalitiesIds?.Any() ?? false)
      query = query.Where(pc => req.MunicipalitiesIds.Contains(pc.MunicipalityId));

    // Filtrado por IDs de Tipos de Vehículo
    if (req.VehicleTypesIds?.Any() ?? false)
      query = query.Where(pc => req.VehicleTypesIds.Contains(pc.VehicleTypeId));

    // Filtrado por costo mínimo
    if (req.CostMin.HasValue)
      query = query.Where(pc => pc.Cost >= req.CostMin.Value);

    // Filtrado por costo máximo
    if (req.CostMax.HasValue)
      query = query.Where(pc => pc.Cost <= req.CostMax.Value);

    // Ejecución de la consulta
    var categories = (await query.ToListAsync(ct)).AsEnumerable();

    // Ordenamiento
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      var propertyInfo = typeof(ReymaniWebApi.Data.Models.ShippingCost).GetProperty(req.SortBy);
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
    var mapper = new ShippingCostMapper();
    var responseData = data.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(new PaginatedResponse<ShippingCostResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
