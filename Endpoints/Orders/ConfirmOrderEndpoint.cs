using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Orders.Requests;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Orders;

public class ConfirmOrderEndpoint : Endpoint<ConfirmOrderRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public ConfirmOrderEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Put("/orders/{id}");
    Summary(s =>
    {
      s.Summary = "Confirm order";
      s.Description = "Confirm an existing order.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(ConfirmOrderRequest req, CancellationToken ct)
  {
    var order = await _dbContext.Orders.FindAsync(req.OrderId, ct);
    if (order?.Status != Data.Models.OrderStatus.InProcess)
      return TypedResults.Conflict();
    if (order is null)
    {
      return TypedResults.NotFound();
    }

    var userAddress = await _dbContext.UserAddresses.FirstOrDefaultAsync(x=> x.UserId ==order.CustomerAddressId);
    var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(x=> x.UserId == req.CourierId);

    if (vehicle is null || userAddress is null)
    {
      return TypedResults.NotFound();
    }

    // Actualiza la orden
    order.CourierId = req.CourierId;
    order.ShippingCost = await calculateShippingCost(userAddress!,vehicle!);
    order.Status = Data.Models.OrderStatus.InPreparation;
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  private async Task<decimal> calculateShippingCost(UserAddress address,Vehicle vehicle)
  {
    var shipCost = await _dbContext.ShippingCosts.FirstOrDefaultAsync(x => x.VehicleTypeId == vehicle.VehicleTypeId && x.MunicipalityId == address.MunicipalityId);
    return shipCost?.Cost ?? 0;
  }
}
