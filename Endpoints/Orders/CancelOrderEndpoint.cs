using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Orders.Requests;

namespace reymani_web_api.Endpoints.Orders;

public class CancelOrderEndpoint : Endpoint<CancelOrderRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public CancelOrderEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Delete("/orders/cancel/{id}");
    Summary(s =>
    {
      s.Summary = "Cancel  order";
      s.Description = "Cancel order.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CancelOrderRequest req, CancellationToken ct)
  {
    var order = await _dbContext.Orders.FindAsync(req.Id, ct);

    if (order is null)
      return TypedResults.NotFound();

    if (order?.Status == Data.Models.OrderStatus.Completed)
      return TypedResults.Conflict();

    order!.Status = Data.Models.OrderStatus.Cancelled;
    _dbContext.Orders.Remove(order);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
