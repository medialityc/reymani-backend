using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.VehiclesTypes.Requests;
using reymani_web_api.Endpoints.VehiclesTypes.Responses;
using reymani_web_api.Services.BlobServices;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.VehiclesTypes;

public class SearchVehicleTypeCourierEndpoint : Endpoint<SearchVehicleTypeCourierRequest, Results<Ok<PaginatedResponse<VehicleTypeResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public SearchVehicleTypeCourierEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehiclesTypes/search");
    Summary(s =>
    {
      s.Summary = "Search types vehicles";
      s.Description = "Search for types vehicles by name or ID with filtering, sorting, and pagination.";
    });
    Roles("Courier");
  }

  public override async Task<Results<Ok<PaginatedResponse<VehicleTypeResponse>>, ProblemDetails>> ExecuteAsync(SearchVehicleTypeCourierRequest req, CancellationToken ct)
  {
    var query = _dbContext.VehicleTypes
      .Where(p => p.IsActive == true)
      .AsNoTracking()
      .AsQueryable();

    //Obtener los costos de envio
    var shippingCosts = await _dbContext.ShippingCosts
        .Include(sc => sc.Municipality)
        .AsNoTracking()
        .ToListAsync(ct);

    // Agrupar los costos de envío por VehicleTypeId
    var shippingCostsByVehicleTypeId = shippingCosts
        .GroupBy(sc => sc.VehicleTypeId)
        .ToDictionary(g => g.Key, g => g.ToList());

    //Filtrado
    if (req.Ids?.Any() ?? false)
      query = query.Where(p => req.Ids.Contains(p.Id));

    if (req.Names?.Any() ?? false)
      query = query.Where(p => req.Names.Any(n =>
          p.Name.ToLower().Contains(n.ToLower().Trim())));

    // Filtrado por capaidad mínima
    if (req.CapacityMin.HasValue)
      query = query.Where(pc => pc.TotalCapacity >= req.CapacityMin.Value);

    // Filtrado por capacidad máxima
    if (req.CapacityMax.HasValue)
      query = query.Where(pc => pc.TotalCapacity <= req.CapacityMax.Value);


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
      var propertyInfo = typeof(VehicleType).GetProperty(req.SortBy);
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
    var mapper = new VehicleTypeMapper();
    var responseData = await Task.WhenAll(data.Select(async u =>
    {
      var resp = mapper.FromEntity(u, shippingCostsByVehicleTypeId);
      if (!string.IsNullOrEmpty(u.Logo))
        resp.Logo = await _blobService.PresignedGetUrl(u.Logo, ct);
      return resp;
    }));

    return TypedResults.Ok(new PaginatedResponse<VehicleTypeResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
