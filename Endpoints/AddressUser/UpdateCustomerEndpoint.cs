using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.AddressUser;

public class UpdateCustomerEndpoint : Endpoint<UpdateCustomerRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public UpdateCustomerEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
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
    // Validar que el nombre no este en uso
    var nameInUse = await BeUniqueName(req.Name, ct);
    if (!nameInUse)
      return TypedResults.Conflict();

    // Verifica si la direccion existe
    var address = await _dbContext.UserAddresses.FindAsync(req.Id, ct);
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

  private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
  {
    // Verifica si ya existe una direccion con el mismo nombre, excluyendo la direccion que se está actualizando
    return !await _dbContext.UserAddresses
        .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
  }
}
