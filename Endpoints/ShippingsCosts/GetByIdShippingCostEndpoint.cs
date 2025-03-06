using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;

using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.ShippingsCost.Responses;
using reymani_web_api.Endpoints.ShippingCost.Request;


namespace reymani_web_api.Endpoints.ShippingsCost;

public class GetByIdShippingCostEndpoint : Endpoint<GetShippingCostByIdRequest, Results<Ok<ShippingCostResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public GetByIdShippingCostEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/shippingcost/{id}");
    Summary(s =>
    {
      s.Summary = "Get shipping cost by Id";
      s.Description = "Retrieves details of a shipping cost by their ID.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<ShippingCostResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetShippingCostByIdRequest req, CancellationToken ct)
  {
    var sc = await _dbContext.ShippingCosts
        .Include(p => p.Municipality)
        .Include(p => p.VehicleType)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (sc is null)
      return TypedResults.NotFound();

    var mapper = new ShippingCostMapper();

    var response = mapper.FromEntity(sc);

    return TypedResults.Ok(response);
  }
}
