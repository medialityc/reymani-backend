using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Vehicles.Response;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Vehicles;

public class GetAllVehiclesSystemAdminEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<VehicleResponse>>, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetAllVehiclesSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/vehicles/admin");
    Summary(s =>
    {
      s.Summary = "Get all vehicles";
      s.Description = "Retrieves a list of all vehicles.";
    });
    //Roles("SystemAdmin");
    AllowAnonymous();
  }

  public override async Task<Results<Ok<IEnumerable<VehicleResponse>>, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new VehicleMapper();

    var vehicles = await _dbContext.Vehicles
      .Include(p => p.VehicleType)
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
