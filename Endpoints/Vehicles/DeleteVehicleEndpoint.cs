using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Vehicles;

public class DeleteVehicleEndpoint : Endpoint<DeleteVehicleRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public DeleteVehicleEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Delete("/vehicles/{id}");
    Summary(s =>
    {
      s.Summary = "Delete vehicle";
      s.Description = "Deletes a vehicle.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(DeleteVehicleRequest req, CancellationToken ct)
  {
    // Verifica si el vehiculo existe
    var vehicle = await _dbContext.Vehicles.FindAsync(req.Id, ct);
    if (vehicle is null)
    {
      return TypedResults.NotFound();
    }

    // Elimina el vehiculo
    _dbContext.Vehicles.Remove(vehicle);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
