using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;

using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.ShippingCost.Request;
using reymani_web_api.Endpoints.ShippingsCost.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.ShippingsCost;

public class CreateShippingCostEndpoint : Endpoint<CreateShippingCostRequest, Results<Created<ShippingCostResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  public CreateShippingCostEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Post("/shippingcost");
    Summary(s =>
    {
      s.Summary = "Create shipping cost";
      s.Description = "Creates a new shipping cost.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Created<ShippingCostResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateShippingCostRequest req, CancellationToken ct)
  {
    var existinShippingCost = await _dbContext.ShippingCosts.FirstOrDefaultAsync(x => x.MunicipalityId==req.MunicipalityId && x.VehicleTypeId == req.VehicleTypeId, ct);
    if (existinShippingCost != null)
    {
      return TypedResults.Conflict();
    }

    var mapper = new ShippingCostMapper();
    var sc = mapper.ToEntity(req);

    // Agrega el nuevo costo de envio a la base de datos
    _dbContext.ShippingCosts.Add(sc);
    await _dbContext.SaveChangesAsync(ct);

    var response = mapper.FromEntity(sc);

    return TypedResults.Created($"/shippingcost/{sc.Id}", response);
  }
}
