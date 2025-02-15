using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Services.BlobServices;

using static FastEndpoints.Ep;



namespace reymani_web_api.Endpoints.Provinces;

public class UpdateProvinceEndpoint : Endpoint<UpdateProvinceRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public UpdateProvinceEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Put("/provinces/{id}");
    Summary(s =>
    {
      s.Summary = "Update province";
      s.Description = "Updates details of an existing province.";
    });
    Roles("SystemAdmin");
    AllowFormData();
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateProvinceRequest req, CancellationToken ct)
  {
    // Validar la solicitud
    var nameInUse = await BeUniqueName(req.Name, ct);
    if (!nameInUse)
      return TypedResults.Conflict();

    // Verifica si la provincia existe
    var province = await _dbContext.Provinces.FindAsync(req.Id, ct);
    if (province is null)
    {
      return TypedResults.NotFound();
    }

    // Actualiza la provincia
    province.Name = req.Name;
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }

  private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
  {
    // Verifica si ya existe una provincia con el mismo nombre, excluyendo la provincia que se está actualizando
    return !await _dbContext.Provinces
        .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellationToken);
  }
}
