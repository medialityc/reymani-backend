using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Municipalities.Responses;

namespace reymani_web_api.Endpoints.Municipalities;

public class CreateMunicipalityEndpoint : Endpoint<CreateMunicipalityRequest, Results<Created<MunicipalityWithNameProvinceResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public CreateMunicipalityEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
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
