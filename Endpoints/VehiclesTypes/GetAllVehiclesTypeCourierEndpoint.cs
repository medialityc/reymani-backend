using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.VehiclesTypes.Responses;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.VehiclesTypes;

public class GetAllVehiclesTypeCourierEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<VehicleTypeResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetAllVehiclesTypeCourierEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehiclesTypes");
    Summary(s =>
    {
      s.Summary = "Get all vehicles types";
      s.Description = "Retrieves a list of all vehicles types with shippings cost.";
    });
    Roles("Courier");
  }

  public override async Task<Results<Ok<IEnumerable<VehicleTypeResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var _mapper = new VehicleTypeMapper();


    // Obtener todos los tipos de vehículos
    var vehicleTypes = await _dbContext.VehicleTypes
        .Where(p=>p.IsActive==true)
        .AsNoTracking()
        .OrderBy(vt => vt.Id)
        .ToListAsync(ct);

    // Obtener todos los costos de envío con el Municipio incluido
    var shippingCosts = await _dbContext.ShippingCosts
        .Include(sc => sc.Municipality) 
        .AsNoTracking() 
        .ToListAsync(ct);

    // Agrupar los costos de envío por VehicleTypeId
    var shippingCostsByVehicleTypeId = shippingCosts
        .GroupBy(sc => sc.VehicleTypeId) 
        .ToDictionary(g => g.Key, g => g.ToList());

    // Mapear los tipos de vehículos a objetos de respuesta usando el VehicleTypeMapper
    var response = await Task.WhenAll(vehicleTypes.Select(async vt =>
    {
      var vehicleTypeResponse = _mapper.FromEntity(vt, shippingCostsByVehicleTypeId);

      // Generar una URL pre firmada para el logo si existe
      if (!string.IsNullOrEmpty(vt.Logo))
      {
        vehicleTypeResponse.Logo = await _blobService.PresignedGetUrl(vt.Logo, ct);
      }

      return vehicleTypeResponse;
    }));

    return TypedResults.Ok(response.AsEnumerable());
  }
}
