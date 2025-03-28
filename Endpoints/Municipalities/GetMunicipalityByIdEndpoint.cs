﻿using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;

using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Municipalities.Responses;


namespace reymani_web_api.Endpoints.Municipalities;

public class GetMunicipalityByIdEndpoint : Endpoint<GetMunicipalityByIdRequest, Results<Ok<MunicipalityWithNameProvinceResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public GetMunicipalityByIdEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
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

  public override async Task<Results<Ok<MunicipalityWithNameProvinceResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetMunicipalityByIdRequest req, CancellationToken ct)
  {
    var municipality = await _dbContext.Municipalities
        .Include(p => p.Province)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (municipality is null)
      return TypedResults.NotFound();

    var mapper = new MunicipalitiesMapper();

    var response = mapper.FromEntity(municipality);

    return TypedResults.Ok(response);
  }
}
