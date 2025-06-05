using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Orders.OrdersItems.Response;
using reymani_web_api.Endpoints.Orders.Requests;
using reymani_web_api.Endpoints.Orders.Responses;
using reymani_web_api.Endpoints.Users.Responses;

using ReymaniWebApi.Data.Models;


namespace reymani_web_api.Endpoints.Orders;

public class CreateOrderEndpoint : Endpoint<CreateOrderRequest, Results<Created<OrderResponse>, Conflict, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public CreateOrderEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Post("/orders");
    Summary(s =>
    {
      s.Summary = "Create order";
      s.Description = "Creates a new order.";
    });
    Roles("Customer", "BusinessAdmin", "SystemAdmin");
  }

  public override async Task<Results<Created<OrderResponse>, Conflict, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateOrderRequest req, CancellationToken ct)
  {
    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      return TypedResults.Unauthorized();


    if ((req.RequiresCourierService && !req.CourierId.HasValue) || (!req.RequiresCourierService && req.CourierId.HasValue))
    {
      //return TypedResults.Conflict();
      AddError("Si requiere mensajeria se requiere de un mensajero. Si no se requiere, no se debe proporcinar uno");
    }


    if (req.RequiresCourierService)
    {
      var courier = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == req.CourierId && u.Role == UserRole.Courier, cancellationToken: ct);
      if (courier == null)
      {
        //return TypedResults.Conflict();
        AddError($"No existe mensajero con id: {req.CourierId}");
      }
    }


    var customer = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == req.CustomerId && u.Role == UserRole.Customer, ct); ;

    if (customer == null)
    {
      // return TypedResults.NotFound();
      AddError($"No existe cliente con id: {req.CustomerId}");
    }
    ThrowIfAnyErrors();
    
  
    var shoppingCart = await _dbContext.ShoppingCarts
                                  .Include(sc => sc.Items!)
                                    .ThenInclude(item => item.Product)
                                  .FirstOrDefaultAsync(x => x.Id == req.ShoppingCartId && x.UserId == customer.Id, ct);


    if (shoppingCart == null)
    {
      return TypedResults.NotFound();
    }

    var mapper = new OrderMapper();
    var mapperItem = new OrderItemMapper();

    var order = mapper.ToEntity(req);
    _dbContext.Orders.Add(order);
    decimal productCostTotal = 0;

    //Llenar la orden
    foreach (var i in shoppingCart.Items!)
    {
      var item = mapperItem.OrderItemFromShoppingCartItem(i, order);
      order.Items?.Add(item);
      decimal discount = 0;
      if (i.Product!.DiscountPrice.HasValue)
        discount = i.Product.Price * i.Product.DiscountPrice.Value;

      productCostTotal += i.Quantity * (i.Product.Price - discount);
    }

    order.TotalProductsCost = productCostTotal;

    shoppingCart.Items.Clear();
    await _dbContext.SaveChangesAsync(ct);

    // Obtener la orden actualizada con todas sus relaciones
    var query = _dbContext.Orders
        .Include(o => o.Items!)
            .ThenInclude(i => i.Product)
        .Include(o => o.Customer)
        .Include(o => o.CustomerAddress);

    if (req.RequiresCourierService)
    {
      query.Include(o => o.Courier);
    }

    var orderWithRelations = await query.FirstOrDefaultAsync(o => o.Id == order.Id, ct);

    var response = mapper.FromEntity(orderWithRelations!);

    // Mapear los ítems de la orden directamente desde la entidad order
    var itemsResponse = orderWithRelations!.Items!.Select(item => mapperItem.FromEntity(item)).ToList();
    response.Items = itemsResponse;

    return TypedResults.Created($"/orders/{order.Id}", response);
  }
}
