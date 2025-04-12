using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Vehicles;

public class UpdateVehicleSystemAdminEndpoint : Endpoint<UpdateCourierAdminRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public UpdateVehicleSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Put("/vehicles/admin/{id}");
    Summary(s =>
    {
      s.Summary = "Update vehicle";
      s.Description = "Updates details of an existing vehicle.";
    });
    Roles("SystemAdmin");
    AllowFormData();
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateCourierAdminRequest req, CancellationToken ct)
  {
    // Verifica si el vehiculo existe
    var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (vehicle is null)
    {
      return TypedResults.NotFound();
    }

    // Validar que el nombre no este en uso
    if (req.Name != null)
    {
      var nameInUse = await BeUniqueName(req.Id, req.Name, vehicle.UserId, ct);
      if (!nameInUse)
        return TypedResults.Conflict();
    }

    // Actualiza las propiedades del vehículo // Mantener el valor actual si es null
    vehicle.Name = req.Name ?? vehicle.Name;
    vehicle.Description = req.Description ?? vehicle.Description;
    vehicle.IsAvailable = req.IsAvailable ?? vehicle.IsAvailable;
    vehicle.VehicleTypeId = req.VehicleTypeId ?? vehicle.VehicleTypeId;
    vehicle.IsActive = req.IsActive ?? vehicle.IsActive;

    //Elimina la foto que habia
    if (!string.IsNullOrEmpty(vehicle.Picture))
      await _blobService.DeleteObject(vehicle.Picture, ct);

    // Agregar nueva imágen solo si se proporciona.
    if (req.Picture != null)
    {
      //Poner la nueva foto
      var fileCode = Guid.NewGuid().ToString();
      string imagePath = await _blobService.UploadObject(req.Picture, fileCode, ct);
      vehicle.Picture = imagePath;
    }
    else
    {
      // Si no se proporciona imagen, establecer Logo a null (como en UpdateUserEndpoint)
      vehicle.Picture = null;
    }

    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  private async Task<bool> BeUniqueName(int currentVehicleId, string name, int idCourier, CancellationToken cancellationToken)
  {
    // Verifica si ya existe un vehículo con el mismo nombre, excluyendo el que se está actualizando
    return !await _dbContext.Vehicles
        .AnyAsync(p => p.Id != currentVehicleId && p.Name.ToLower() == name.ToLower() && p.UserId == idCourier, cancellationToken);
  }
}
