using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Endpoints.Vehicles.Response;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Vehicles;

public class CreateCourierEndpoint : Endpoint<CreateVehicleCourierRequest, Results<Created<VehicleResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public CreateCourierEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Post("/vehicles");
    Summary(s =>
    {
      s.Summary = "Create a vehicle";
      s.Description = "Creates a new vehicle.";
    });
    Roles("Courier");
    AllowFormData();
  }

  public override async Task<Results<Created<VehicleResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateVehicleCourierRequest req, CancellationToken ct)
  {

    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      return TypedResults.Unauthorized();
    

    var existingName = await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(req.Name.ToLower()), ct);
    
    if (existingName != null)
      return TypedResults.Conflict();
    

    var mapper = new VehicleMapper();
    var vehicle = mapper.ToEntity(req);

 
    //Poner la nueva foto
    var fileCode = Guid.NewGuid().ToString();
    string imagePath = await _blobService.UploadObject(req.Picture, fileCode, ct);
    vehicle.Picture = imagePath;

    //Agrega id courier
    vehicle.UserId = userId;

    // Agrega el nuevo vehiculo a la base de datos
    _dbContext.Vehicles.Add(vehicle);
    await _dbContext.SaveChangesAsync(ct);


    var response = mapper.FromEntity(vehicle);

    return TypedResults.Created($"/vehicles/{vehicle.Id}", response);
  }
}
