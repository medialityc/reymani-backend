using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Endpoints.Vehicles.Response;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Vehicles;

public class GetByIdVehicleSystemAdminEndpoint : Endpoint<GetByIdRequest, Results<Ok<VehicleResponse>, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetByIdVehicleSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehicles/admin/{id}");
    Summary(s =>
    {
      s.Summary = "Get vehicle by Id";
      s.Description = "Retrieves details of a vehicle by their ID.";
    });
    //Roles("SystemAdmin");
    AllowAnonymous();
  }

  public override async Task<Results<Ok<VehicleResponse>, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(GetByIdRequest req, CancellationToken ct)
  {
    var vehicle = await _dbContext.Vehicles
        .Include(p => p.VehicleType)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (vehicle is null)
      return TypedResults.NotFound();


    var mapper = new VehicleMapper();

    var response = mapper.FromEntity(vehicle);
    if (!string.IsNullOrEmpty(vehicle.Picture))
      response.Picture = await _blobService.PresignedGetUrl(vehicle.Picture, ct);

    return TypedResults.Ok(response);
  }
}
