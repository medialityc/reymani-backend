using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Endpoints.Vehicles.Response;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Vehicles;

public class GetByIdCourierEndpoint : Endpoint<GetByIdRequest, Results<Ok<VehicleResponse>, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetByIdCourierEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehicles/{id}");
    Summary(s =>
    {
      s.Summary = "Get vehicle by Id";
      s.Description = "Retrieves details of a vehicle by their ID.";
    });
    Roles("Courier");
  }

  public override async Task<Results<Ok<VehicleResponse>, NotFound,UnauthorizedHttpResult,ForbidHttpResult, ProblemDetails>> ExecuteAsync(GetByIdRequest req, CancellationToken ct)
  {
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
      return TypedResults.Unauthorized();
    }

    var vehicle = await _dbContext.Vehicles
        .Where(p => p.IsActive == true)
        .Include(p => p.VehicleType)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id && p.UserId == userId, ct);

    if (vehicle is null)
      return TypedResults.NotFound();


    var mapper = new VehicleMapper();

    var response = mapper.FromEntity(vehicle);
    if (!string.IsNullOrEmpty(vehicle.Picture))
      response.Picture = await _blobService.PresignedGetUrl(vehicle.Picture, ct);

    return TypedResults.Ok(response);
  }
}
