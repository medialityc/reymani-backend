﻿using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ShippingsCosts.Request;

namespace reymani_web_api.Endpoints.ShippingsCost;

public class UpdateShippingCostEndpoint : Endpoint<UpdateShippingCostRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  public UpdateShippingCostEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Put("/shippingcost/{id}");
    Summary(s =>
    {
      s.Summary = "Update shipping cost";
      s.Description = "Updates cost of an existing shipping cost.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateShippingCostRequest req, CancellationToken ct)
  {
    // Verifica si el costo de envio existe
    var sc = await _dbContext.ShippingCosts.FindAsync(req.Id, ct);
    if (sc is null)
    {
      return TypedResults.NotFound();
    }

    // Actualiza el costo de envio
    sc.Cost = req.Cost;
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
