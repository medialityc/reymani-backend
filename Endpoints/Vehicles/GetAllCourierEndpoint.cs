using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Vehicles.Response;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;


namespace reymani_web_api.Endpoints.Vehicles;

public class GetAllCourierEndpoint : EndpointWithoutRequest< Results<Ok<IEnumerable<VehicleResponse>>,UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetAllCourierEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehicles");
    Summary(s =>
    {
      s.Summary = "Get all active vehicles";
      s.Description = "Retrieves a list of all vehicles.";
    });
    Roles("Courier");
  }

  public override async Task<Results<Ok<IEnumerable<VehicleResponse>>, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
      return TypedResults.Unauthorized();
    }

    var mapper = new VehicleMapper();

    var vehicles = await _dbContext.Vehicles
      .Where(p => p.IsActive == true && p.UserId == userId)
      .Include(p=> p.VehicleType)
      .AsNoTracking()
      .ToListAsync(ct);


    
    var response = await Task.WhenAll(vehicles.Select(async u =>
    {
      var resp = mapper.FromEntity(u);
      if (!string.IsNullOrEmpty(u.Picture))
        resp.Picture = await _blobService.PresignedGetUrl(u.Picture, ct);
      return resp;
    }));

    return TypedResults.Ok(response.AsEnumerable());
  }

}
