using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.VehiclesTypes.Requests;
using reymani_web_api.Endpoints.VehiclesTypes.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.VehiclesTypes;

public class GetByIdVehicleTypeAdminEndpoint : Endpoint<GetByIdRequest, Results<Ok<VehicleTypeResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetByIdVehicleTypeAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehiclesTypes/admin/{id}");
    Summary(s =>
    {
      s.Summary = "Get vehicle type by Id";
      s.Description = "Retrieves details of a vehicle type by their ID.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<VehicleTypeResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetByIdRequest req, CancellationToken ct)
  {
    var _mapper = new VehicleTypeMapper();

    var vehicleType = await _dbContext.VehicleTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(vt => vt.Id == req.Id, ct);


    if (vehicleType == null)
    {
      return TypedResults.NotFound();
    }

    // Obtener los costos de envío asociados al tipo de vehículo
    var shippingCosts = await _dbContext.ShippingCosts
        .Include(sc => sc.Municipality)
        .Where(sc => sc.VehicleTypeId == req.Id)
        .ToListAsync(ct);

    // Mapear el tipo de vehículo y los costos de envío a la respuesta
    var response = _mapper.FromEntity(vehicleType, shippingCosts);

    // Generar una URL pre firmada para el logo si existe
    if (!string.IsNullOrEmpty(vehicleType.Logo))
    {
      response.Logo = await _blobService.PresignedGetUrl(vehicleType.Logo, ct);
    }

    return TypedResults.Ok(response);
  }
}
