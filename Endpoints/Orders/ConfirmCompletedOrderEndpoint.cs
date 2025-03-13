using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Orders.Requests;

namespace reymani_web_api.Endpoints.Orders;

public class ConfirmCompletedOrderEndpoint : Endpoint<ConfirmCompletedOrderRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public ConfirmCompletedOrderEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Put("/orders/completed/{id}");
    Summary(s =>
    {
      s.Summary = "Confirm completed order";
      s.Description = "Confirm completed order.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(ConfirmCompletedOrderRequest req, CancellationToken ct)
  {
    var order = await _dbContext.Orders.FindAsync(req.Id, ct);

    if (order is null)
      return TypedResults.NotFound();

    if (order?.Status != Data.Models.OrderStatus.Delivered)
      return TypedResults.Conflict();

    order.Status = Data.Models.OrderStatus.Completed;
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
