using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Services.BlobServices;


namespace reymani_web_api.Endpoints.Vehicles;

public class UpdateCourierEndpoint : Endpoint<UpdateCourierRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public UpdateCourierEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Put("/vehicles/{id}");
    Summary(s =>
    {
      s.Summary = "Update vehicle";
      s.Description = "Updates details of an existing vehicle.";
    });
    //Roles("Courier");
    AllowFormData();
    AllowAnonymous();
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateCourierRequest req, CancellationToken ct)
  {
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
      return TypedResults.Unauthorized();
    }

    // Validar que el nombre no este en uso
    if (req.Name !=null)
    {
      var nameInUse = await BeUniqueName(req.Name, ct);
      if (!nameInUse)
        return TypedResults.Conflict();
    }

    // Verifica si el vehiculo existe
    var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(p=> p.Id==req.Id && p.UserId==userId, ct);

    if (vehicle is null)
    {
      return TypedResults.NotFound();
    }

    // Actualiza las propiedades del vehículo // Mantener el valor actual si es null
    vehicle.Name = req.Name ?? vehicle.Name;
    vehicle.Description = req.Description ?? vehicle.Description;
    vehicle.IsAvailable = req.IsAvailable ?? vehicle.IsAvailable; 
    vehicle.VehicleTypeId = req.VehicleTypeId ?? vehicle.VehicleTypeId;
    vehicle.UserId = userId;


    // Agregar nueva imágen solo si se proporciona.
    if (req.Picture != null)
    {
      //Elimina la foto que habia
      if (!string.IsNullOrEmpty(vehicle.Picture))
        await _blobService.DeleteObject(vehicle.Picture, ct);

      //Poner la nueva foto
      var fileCode = Guid.NewGuid().ToString();
      string imagePath = await _blobService.UploadObject(req.Picture, fileCode, ct);
      vehicle.Picture=imagePath;
    }
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
  {
    // Verifica si ya existe un courier con el mismo nombre, excluyendo el que se está actualizando
    return !await _dbContext.Vehicles
        .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
  }

}
