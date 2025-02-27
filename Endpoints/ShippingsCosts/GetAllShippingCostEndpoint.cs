using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;

using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.ShippingsCost.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.ShippingsCost;

public class GetAllShippingCostEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<ShippingCostResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetAllShippingCostEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/shippingcost");
    Summary(s =>
    {
      s.Summary = "Get all shipping cost";
      s.Description = "Retrieves a list of all shipping cost with municipies.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<IEnumerable<ShippingCostResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new ShippingCostMapper();


    var sc = await _dbContext.ShippingCosts
        .Include(p => p.Municipality)
        .Include(p => p.VehicleType)
        .AsNoTracking()
        .OrderBy(u => u.Id)
        .ToListAsync(ct);

    var response = sc.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(response.AsEnumerable());
  }
}
