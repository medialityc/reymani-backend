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

public class CreateMunicipalityEndpoint : Endpoint<CreateMunicipalityRequest, Results<Created<MunicipalityWithNameProvinceResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public CreateMunicipalityEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Post("/municipalities");
    Summary(s =>
    {
      s.Summary = "Create municipality";
      s.Description = "Creates a new municipality.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Created<MunicipalityWithNameProvinceResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, NotFound, ProblemDetails>> ExecuteAsync(CreateMunicipalityRequest req, CancellationToken ct)
  {
    var existingMunicipality = await _dbContext.Municipalities.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(req.Name.ToLower()), ct);
    if (existingMunicipality != null)
    {
      return TypedResults.Conflict();
    }

    var existingProvince = await _dbContext.Provinces.FirstOrDefaultAsync(x => x.Id==req.ProvinceId, ct);
    if (existingProvince == null)
      return TypedResults.NotFound();
    


    var mapper = new MunicipalitiesMapper();
    var municipality = mapper.ToEntity(req);

    // Agrega el nuevo municipio a la base de datos
    _dbContext.Municipalities.Add(municipality);
    await _dbContext.SaveChangesAsync(ct);


    var response = mapper.FromEntity(municipality);


    return TypedResults.Created($"/municipalities/{municipality.Id}", response);
  }
}
