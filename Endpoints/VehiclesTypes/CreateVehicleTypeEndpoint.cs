using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.VehiclesTypes.Requests;
using reymani_web_api.Endpoints.VehiclesTypes.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.VehiclesTypes;

public class CreateVehicleTypeEndpoint : Endpoint<CreateVehicleTypeRequest, Results<Created<VehicleTypeResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public CreateVehicleTypeEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Post("/vehiclesTypes");
    Summary(s =>
    {
      s.Summary = "Create a vehicle type";
      s.Description = "Creates a new vehicle type.";
    });
    Roles("SystemAdmin");
    AllowFormData();
  }

  public override async Task<Results<Created<VehicleTypeResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateVehicleTypeRequest req, CancellationToken ct)
  {
    if (await BeUniqueName(req.Name,ct))
      return TypedResults.Conflict();


    var mapper = new VehicleTypeMapper();
    var vehicleType = mapper.ToEntity(req);


    //Poner la nueva foto
    var fileCode = Guid.NewGuid().ToString();
    string imagePath = await _blobService.UploadObject(req.Logo, fileCode, ct);
    vehicleType.Logo = imagePath;


    // Agrega el nuevo tipo de vehiculo a la base de datos
    _dbContext.VehicleTypes.Add(vehicleType);
    await _dbContext.SaveChangesAsync(ct);

    var response = mapper.FromEntity(vehicleType);

    return TypedResults.Created($"/vehiclesTypes/{vehicleType.Id}", response);
  }
  private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
  {
    // Verifica si ya existe un tipo de vehiculo con el mismo nombre, excluyendo el que se está actualizando
    return !await _dbContext.VehicleTypes
        .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
  }
}
