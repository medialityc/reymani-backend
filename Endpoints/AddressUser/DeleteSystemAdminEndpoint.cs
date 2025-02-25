using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.AddressUser.Requests;

namespace reymani_web_api.Endpoints.AddressUser;

public class DeleteSystemAdminEndpoint : Endpoint<DeleteUserAddressRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public DeleteSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Delete("/userAddress/{id}");
    Summary(s =>
    {
      s.Summary = "Delete userAddress";
      s.Description = "Deletes a userAddress if it is not in use by any entity.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(DeleteUserAddressRequest req, CancellationToken ct)
  {
    // Verifica si la direccion existe
    var userAddress = await _dbContext.UserAddresses.FindAsync(req.Id, ct);
    if (userAddress is null)
    {
      return TypedResults.NotFound();
    }

    // Verifica si la direccion está en uso por carritos de venta
    var isInUse = await _dbContext.ShoppingCarts.AnyAsync(m => m.UserAddressId == req.Id, ct);
    if (isInUse)
    {
      return TypedResults.Conflict();
    }

    // Verifica si la direccion está en uso por ordenes
    var isInUseOrders = await _dbContext.Orders.AnyAsync(m => m.CustomerAddressId == req.Id, ct);
    if (isInUseOrders)
    {
      return TypedResults.Conflict();
    }

    // Elimina la direccion de usuario
    _dbContext.UserAddresses.Remove(userAddress);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
