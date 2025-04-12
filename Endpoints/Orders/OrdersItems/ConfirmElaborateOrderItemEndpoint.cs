using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Orders.OrdersItems.Requests;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Orders.OrdersItems;

public class ConfirmElaborateOrderItemEndpoint : Endpoint<ConfirmElaborateOrderItemRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public ConfirmElaborateOrderItemEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Put("/orders/orderitems/{id}");
    Summary(s =>
    {
      s.Summary = "Confirm elaborate item in order";
      s.Description = "Confirm elaborate item in existing order.";
    });
    Roles("BusinessAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(ConfirmElaborateOrderItemRequest req, CancellationToken ct)
  {
    var order = await _dbContext.Orders
      .Include(o=> o.Items)
      .FirstOrDefaultAsync(o=> o.Id==req.OrderId, ct);

    if (order is null)
      return TypedResults.NotFound();

    var item = order.Items!.FirstOrDefault(x => x.Id == req.OrderItemId);
    if (item is null)
      return TypedResults.NotFound();

    if (item.Status != OrderItemStatus.InPreparation)
      return TypedResults.Conflict();

    //Revisa que el pedido se pueda cambiar a completado
    if (order.Status != OrderStatus.InPreparation)
      return TypedResults.Conflict();
    item.Status = OrderItemStatus.InPickup;

    // Actualiza el pedido
    if (checkOrder(order.Items!.ToList()))
    {
      order.Status = OrderStatus.InPickup;
    }
    

    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  public bool checkOrder(List<OrderItem> item)
  {
    foreach (var i in item)
    {
      if (i.Status != OrderItemStatus.InPickup)
        return false;
    }
    return true;
  }
}
