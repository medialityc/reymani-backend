using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.VehiclesTypes.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.VehiclesTypes;

public class DeleteVehicleTypeEndpoint : Endpoint<DeleteVehicleTypeRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public DeleteVehicleTypeEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Delete("/vehiclesTypes/{id}");
    Summary(s =>
    {
      s.Summary = "Delete vehicle type";
      s.Description = "Deletes a vehicle type.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(DeleteVehicleTypeRequest req, CancellationToken ct)
  {
    // Verifica si el tipo de vehículo existe
    var vehicleType = await _dbContext.VehicleTypes.FindAsync(req.Id, ct);
    if (vehicleType is null)
    {
      return TypedResults.NotFound(); // Tipo de vehículo no encontrado
    }

    // Verifica si el tipo de vehículo está en uso en la tabla Vehicles
    var isUsedInVehicles = await _dbContext.Vehicles
        .AnyAsync(v => v.VehicleTypeId == req.Id, ct);

    if (isUsedInVehicles)
    {
      return TypedResults.Conflict(); //Conflicto, el tipo de vehículo está en uso en Vehicles
    }

    // Elimina la foto del tipo de vehículo
    if (!string.IsNullOrEmpty(vehicleType.Logo))
    {
      await _blobService.DeleteObject(vehicleType.Logo, ct);
    }

    // Si no está en uso, procede a eliminar el tipo de vehículo
    _dbContext.VehicleTypes.Remove(vehicleType);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
