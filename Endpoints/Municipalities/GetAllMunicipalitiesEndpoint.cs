﻿using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Municipalities.Responses;


namespace reymani_web_api.Endpoints.Municipalities;

public class GetAllMunicipalitiesEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<MunicipalityWithNameProvinceResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;


  public GetAllMunicipalitiesEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/municipalities");
    Summary(s =>
    {
      s.Summary = "Get all municipalities";
      s.Description = "Retrieves a list of all municipies.";
    });
    AllowAnonymous();
  }

  public override async Task<Results<Ok<IEnumerable<MunicipalityWithNameProvinceResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new MunicipalitiesMapper();


    var municipalities = await _dbContext.Municipalities
        .Include(p=>p.Province)
        .AsNoTracking()
        .OrderBy(u => u.Id)
        .ToListAsync(ct);

    var response = municipalities.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(response.AsEnumerable());
  }
}
