using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Municipalities;

public class DeleteMunicipalityEndpoint : Endpoint<DeleteMunicipalityRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public DeleteMunicipalityEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Delete("/municipalities/{id}");
    Summary(s =>
    {
      s.Summary = "Delete province";
      s.Description = "Deletes a municipality if it is not in use by any entity.";
    });
    AllowAnonymous();
    //Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(DeleteMunicipalityRequest req, CancellationToken ct)
  {
    // Verifica si el municipio existe
    var municipality = await _dbContext.Municipalities.FindAsync(req.Id, ct);
    if (municipality is null)
    {
      return TypedResults.NotFound();
    }

    // Verifica si el municipio  está en uso por negocios
    var isInUseBusinesses = await _dbContext.Businesses.AnyAsync(b => b.MunicipalityId == req.Id, ct);

    // Verifica si el municipio  está en uso por direcciones de usuarios
    var isInUseUserAdresses = await _dbContext.UserAddresses.AnyAsync(ua => ua.MunicipalityId == req.Id, ct);

    // Verifica si el municipio  está en uso por costos de envio
    var isInUseShippingCosts = await _dbContext.ShippingCosts.AnyAsync(s => s.MunicipalityId == req.Id, ct);

    if (isInUseBusinesses|| isInUseUserAdresses|| isInUseShippingCosts)
    {
      return TypedResults.Conflict();
    }

    // Elimina el municipio 
    _dbContext.Municipalities.Remove(municipality);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
