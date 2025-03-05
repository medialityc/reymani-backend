using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.VehiclesTypes.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.VehiclesTypes;

public class UpdateVehicleTypeEndpoint : Endpoint<UpdateVehicleTypeRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public UpdateVehicleTypeEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Put("/vehiclesTypes/{id}");
    Summary(s =>
    {
      s.Summary = "Update vehicle type";
      s.Description = "Updates details of an existing vehicle type.";
    });
    Roles("SystemAdmin");
    AllowFormData();
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateVehicleTypeRequest req, CancellationToken ct)
  {
    // Verifica si el tipo vehiculo existe
    var vehicleType = await _dbContext.VehicleTypes.FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (vehicleType is null)
    {
      return TypedResults.NotFound();
    }

    if(req.Name!=null)
      if (await NameAlreadyExists(req.Name, req.Id, ct))
        return TypedResults.Conflict();

    // Actualiza las propiedades del tipo vehículo // Mantener el valor actual si es null
    vehicleType.Name = req.Name ?? vehicleType.Name;
    vehicleType.TotalCapacity= req.TotalCapacity ?? vehicleType.TotalCapacity;
    vehicleType.IsActive = req.IsActive ?? vehicleType.IsActive;

    // Siempre elimina la foto que había (como en UpdateUserEndpoint)
    if (!string.IsNullOrEmpty(vehicleType.Logo))
      await _blobService.DeleteObject(vehicleType.Logo, ct);

    // Agregar nueva imágen solo si se proporciona
    if (req.Logo != null)
    {
        //Poner la nueva foto
        var fileCode = Guid.NewGuid().ToString();
        string imagePath = await _blobService.UploadObject(req.Logo, fileCode, ct);
        vehicleType.Logo = imagePath;
    }
    else
    {
        // Si no se proporciona imagen, establecer Logo a null (como en UpdateUserEndpoint)
        vehicleType.Logo = null;
    }
    
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  private async Task<bool> NameAlreadyExists(string name, int currentTypeId, CancellationToken cancellationToken)
  {
    // Verifica si ya existe un tipo de vehiculo con el mismo nombre, excluyendo el que se está actualizando
    return await _dbContext.VehicleTypes
        .AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != currentTypeId, cancellationToken);
  }
}
