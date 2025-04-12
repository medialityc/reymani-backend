using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Endpoints.Vehicles.Response;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Vehicles;

public class CreateVehicleSystemAdminEndpoint : Endpoint<CreateVehicleAdminRequest, Results<Created<VehicleResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public CreateVehicleSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Post("/vehicles/admin");
    Summary(s =>
    {
      s.Summary = "Create a vehicle";
      s.Description = "Creates a new vehicle.";
    });
    Roles("SystemAdmin");
    AllowFormData();
  }

  public override async Task<Results<Created<VehicleResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateVehicleAdminRequest req, CancellationToken ct)
  {
    var existingName = await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(req.Name.ToLower()) && x.UserId==req.IdCourier, ct);
    
    if (existingName != null)
      return TypedResults.Conflict();
    
    var mapper = new VehicleMapper();
    var vehicle = mapper.ToEntityAdmin(req);


    //Poner la nueva foto
    var fileCode = Guid.NewGuid().ToString();
    string imagePath = await _blobService.UploadObject(req.Picture, fileCode, ct);
    vehicle.Picture = imagePath;

    // Agrega el nuevo vehiculo a la base de datos
    _dbContext.Vehicles.Add(vehicle);
    await _dbContext.SaveChangesAsync(ct);


    var response = mapper.FromEntity(vehicle);


    return TypedResults.Created($"/vehicles/{vehicle.Id}", response);
  }
}
