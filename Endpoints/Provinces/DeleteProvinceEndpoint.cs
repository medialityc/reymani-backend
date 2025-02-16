using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Provinces;

public class DeleteProvinceEndpoint : Endpoint<DeleteProvinceRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public DeleteProvinceEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Delete("/provinces/{id}");
    Summary(s =>
    {
      s.Summary = "Delete province";
      s.Description = "Deletes a province if it is not in use by any municipalities.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(DeleteProvinceRequest req, CancellationToken ct)
  {
    // Verifica si la provincia existe
    var province = await _dbContext.Provinces.FindAsync(req.Id, ct);
    if (province is null)
    {
      return TypedResults.NotFound();
    }

    // Verifica si la provincia está en uso por municipios
    var isInUse = await _dbContext.Municipalities.AnyAsync(m => m.ProvinceId == req.Id, ct);
    if (isInUse)
    {
      return TypedResults.Conflict();
    }

    // Elimina la provincia
    _dbContext.Provinces.Remove(province);
    await _dbContext.SaveChangesAsync(ct);

    return TypedResults.Ok();
  }
}
