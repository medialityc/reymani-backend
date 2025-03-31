using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Endpoints.Vehicles.Response;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Vehicles;

public class SearchVehicleSystemAdminEndpoint : Endpoint<SearchVehiclesRequest, Results<Ok<PaginatedResponse<VehicleResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public SearchVehicleSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehicles/admin/search");
    Summary(s =>
    {
      s.Summary = "Search vehicles";
      s.Description = "Search for vehicles by name or ID with filtering, sorting, and pagination.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<PaginatedResponse<VehicleResponse>>, ProblemDetails>> ExecuteAsync(SearchVehiclesRequest req, CancellationToken ct)
  {
    var query = _dbContext.Vehicles
        .AsNoTracking()
        .Include(p => p.VehicleType)
        .AsQueryable();

    // Filtrado
    if (req.Ids?.Any() ?? false)
      query = query.Where(p => req.Ids.Contains(p.Id));

    if (req.UserIds?.Any() ?? false)
      query = query.Where(p => req.UserIds.Contains(p.UserId));

    if (req.TypesVehicle?.Any() ?? false)
      query = query.Where(p => req.TypesVehicle.Contains(p.VehicleTypeId));

    if (req.Names?.Any() ?? false)
      query = query.Where(p => req.Names.Any(n => p.Name.ToLower().Contains(n.ToLower().Trim())));

    if (req.Descriptions?.Any() ?? false)
      query = query.Where(p => p.Description != null && req.Descriptions.Any(d => p.Description.ToLower().Contains(d.ToLower().Trim())));

    if (req.IsAvailable.HasValue)
      query = query.Where(p => p.IsAvailable == req.IsAvailable.Value);

    if (req.Search is not null)
    {
      var search = req.Search.ToLower().Trim();
      query = query.Where(pc => pc.Name.ToLower().Contains(search));
    }

    // Ejecutar la consulta sin ordenamiento (traer datos a memoria)
    var vehicles = await query.ToListAsync(ct);

    // Ordenamiento del lado del cliente (en memoria)
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      var propertyInfo = typeof(Vehicle).GetProperty(req.SortBy);
      if (propertyInfo != null)
      {
        vehicles = req.IsDescending ?? false
            ? vehicles.OrderByDescending(u => propertyInfo.GetValue(u)).ToList() // Orden descendente
            : vehicles.OrderBy(u => propertyInfo.GetValue(u)).ToList(); // Orden ascendente
      }
    }

    // Paginación en memoria
    var totalCount = vehicles.Count;
    var data = vehicles
        .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
        .Take(req.PageSize ?? 10)
        .ToList();

    // Mapeo de respuesta
    var mapper = new VehicleMapper();
    var responseData = await Task.WhenAll(data.Select(async u =>
    {
      var resp = mapper.FromEntity(u);
      if (!string.IsNullOrEmpty(u.Picture))
        resp.Picture = await _blobService.PresignedGetUrl(u.Picture, ct);
      return resp;
    }));

    return TypedResults.Ok(new PaginatedResponse<VehicleResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
