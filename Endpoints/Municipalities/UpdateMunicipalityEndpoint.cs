using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Municipalities;

public class UpdateMunicipalityEndpoint : Endpoint<UpdateMunicipalityRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{

  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public UpdateMunicipalityEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Put("/municipalities/{id}");
    Summary(s =>
    {
      s.Summary = "Update municipality";
      s.Description = "Updates details of an existing municipality.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateMunicipalityRequest req, CancellationToken ct)
  {
    // Validar la solicitud
    var nameInUse = await BeUniqueName(req.Name, ct);
    if (!nameInUse)
      return TypedResults.Conflict();

    // Verifica si la provincia existe
    var province = await _dbContext.Provinces.FindAsync(req.ProvinceId, ct);
    if (province is null)
    {
      return TypedResults.NotFound();
    }

    // Verifica si el municipio existe
    var municipality = await _dbContext.Municipalities.FindAsync(req.Id, ct);
    if (municipality is null)
    {
      return TypedResults.NotFound();
    }

    var mapper = new MunicipalitiesMapper();
    municipality=mapper.ToEntity(req, municipality,province);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
  {
    // Verifica si ya existe un municipio con el mismo nombre, excluyendo el municipio que se está actualizando
    return !await _dbContext.Municipalities
        .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
  }
}
