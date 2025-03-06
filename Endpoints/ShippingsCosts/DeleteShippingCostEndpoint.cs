using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.ShippingCost.Request;


namespace reymani_web_api.Endpoints.ShippingsCost;

public class DeleteShippingCostEndpoint : Endpoint<DeleteShippingCostRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public DeleteShippingCostEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Delete("/shippingcost/{id}");
    Summary(s =>
    {
      s.Summary = "Delete shipping cost";
      s.Description = "Deletes a shipping costs.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(DeleteShippingCostRequest req, CancellationToken ct)
  {
    // Verifica si el costo de envio existe
    var sc = await _dbContext.ShippingCosts.FindAsync(req.Id, ct);
    if (sc is null)
    {
      return TypedResults.NotFound();
    }


    // Elimina el costo de envio
    _dbContext.ShippingCosts.Remove(sc);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
