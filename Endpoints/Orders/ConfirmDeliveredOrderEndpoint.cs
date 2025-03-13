using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Orders.Requests;

namespace reymani_web_api.Endpoints.Orders
{
  public class ConfirmDeliveredOrderEndpoint : Endpoint<ConfirmDeliveredOrderRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;


    public ConfirmDeliveredOrderEndpoint(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public override void Configure()
    {
      Put("/orders/delivered/{id}");
      Summary(s =>
      {
        s.Summary = "Confirm delivered order";
        s.Description = "Confirm delivered order.";
      });
      Roles("Courier");
    }

    public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(ConfirmDeliveredOrderRequest req, CancellationToken ct)
    {
      var order = await _dbContext.Orders.FindAsync(req.Id, ct);

      if (order is null)
        return TypedResults.NotFound();

      if (order?.Status != Data.Models.OrderStatus.OnTheWay)
        return TypedResults.Conflict();

      order.Status = Data.Models.OrderStatus.Delivered;
      await _dbContext.SaveChangesAsync(ct);

      return TypedResults.Ok();
    }
  }
}
