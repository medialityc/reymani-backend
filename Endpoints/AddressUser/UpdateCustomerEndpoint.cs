using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Requests;

namespace reymani_web_api.Endpoints.AddressUser;

public class UpdateCustomerEndpoint : Endpoint<UpdateCustomerRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public UpdateCustomerEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Put("/userAddress/{id}");
    Summary(s =>
    {
      s.Summary = "Update userAddress";
      s.Description = "Updates details of an existing userAddress.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateCustomerRequest req, CancellationToken ct)
  {
    var userIdClaim = User.Claims.First(c => c.Type == "Id");
    int userId = int.Parse(userIdClaim.Value);
    
    // Validar que el nombre no este en uso
    var nameInUse = await BeUniqueName(req.Name, userId, req.Id, ct);
    if (!nameInUse)
      return TypedResults.Conflict();
    
    // Verifica si la direccion existe
    var address = await _dbContext.UserAddresses.FirstOrDefaultAsync(x => x.Id == req.Id && x.UserId == userId, ct);
    if (address is null)
    {
      return TypedResults.NotFound();
    }

    // Actualiza la direccion
    address.Name = req.Name;
    address.Address = req.Address;
    address.Notes = req.Note;
    address.IsActive = req.IsActive;

    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  private async Task<bool> BeUniqueName(string name, int userId, int id, CancellationToken cancellationToken)
  {
    // Verifica si ya existe una direccion con el mismo nombre, excluyendo la direccion que se está actualizando
    return !await _dbContext.UserAddresses
        .AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.UserId == userId && p.Id != id && p.IsActive, cancellationToken);
  }
}
