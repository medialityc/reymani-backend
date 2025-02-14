using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Business;

public class GetAllBusinessSystemAdminEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<BusinessSystemAdminResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetAllBusinessSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/business/system-admin");
    Summary(s =>
    {
      s.Summary = "Get all  businesses for system admin";
      s.Description = "Retrieves a list of businesses for system admin.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<IEnumerable<BusinessSystemAdminResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var businesses = _dbContext.Businesses
      .Include(b => b.Municipality)
      .ThenInclude(m => m!.Province)
      .Include(b => b.User)
      .OrderBy(b => b.Id)
      .AsEnumerable();

    var mapper = new BusinessMapper();
    var response = await Task.WhenAll(businesses.Select(async b =>
    {
      var res = mapper.FromEntity(b);

      if (!string.IsNullOrEmpty(b.Logo))
        res.Logo = await _blobService.PresignedGetUrl(b.Logo, ct);

      if (!string.IsNullOrEmpty(b.Banner))
        res.Banner = await _blobService.PresignedGetUrl(b.Banner, ct);

      return res;
    }));

    return TypedResults.Ok(response.AsEnumerable());
  }
}