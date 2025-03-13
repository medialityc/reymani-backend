using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Data.Models;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.OrdersItems.Requests;
using ReymaniWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Endpoints.OrdersItems
{
  public class ConfirmPickUpOrderItemEndpoint : Endpoint<ConfirmPickUpOrderItemRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;


    public ConfirmPickUpOrderItemEndpoint(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public override void Configure()
    {
      Put("/orderitems/pickup/{id}");
      Summary(s =>
      {
        s.Summary = "Confirm pick up item in order";
        s.Description = "Confirm pick up item in existing order.";
      });
      Roles("Courier");
    }

    public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(ConfirmPickUpOrderItemRequest req, CancellationToken ct)
    {
      var order = await _dbContext.Orders
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.Id == req.OrderId, ct);

      if (order is null)
        return TypedResults.NotFound();

      var item = order.Items!.FirstOrDefault(x => x.Id == req.OrderItemId);
      if (item is null)
        return TypedResults.NotFound();

      if (item.Status != Data.Models.OrderStatus.InPickup)
        return TypedResults.Conflict();

      //Revisa que el pedido se pueda cambiar a recogido
      if (order.Status != OrderStatus.InPickup)
        return TypedResults.Conflict();

      item.Status = Data.Models.OrderStatus.OnTheWay;
      // Actualiza el pedido
      if (checkOrder(order.Items!.ToList()))
      {
        order.Status = Data.Models.OrderStatus.OnTheWay;
      }


      await _dbContext.SaveChangesAsync(ct);

      return TypedResults.Ok();
    }

    public bool checkOrder(List<OrderItem> item)
    {
      foreach (var i in item)
      {
        if (i.Status != Data.Models.OrderStatus.InPickup)
          return false;
      }
      return true;
    }
  }
}
