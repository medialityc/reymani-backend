using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;

using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Municipalities;

public class GetMunicipalityByIdEndpoint : Endpoint<GetMunicipalitieByIdRequest, Results<Ok<MunicipalitieResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetMunicipalityByIdEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/municipalities/{id}");
    Summary(s =>
    {
      s.Summary = "Get municipality by Id";
      s.Description = "Retrieves details of a municipality by their ID.";
    });
    AllowAnonymous();
  }

  public override async Task<Results<Ok<MunicipalitieResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetMunicipalitieByIdRequest req, CancellationToken ct)
  {
    var municipality = await _dbContext.Municipalities
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (municipality is null)
      return TypedResults.NotFound();

    var mapper = new MunicipalitiesMapper();

    var response = mapper.FromEntity(municipality);

    return TypedResults.Ok(response);
  }
}
