using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Endpoints.Business.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Business
{
  public class GetBusinessByIdSystemAdminEndpoint : Endpoint<GetBusinessByIdRequest, Results<Ok<BusinessSystemAdminResponse>, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetBusinessByIdSystemAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/business/system-admin/{Id}");
      Summary(s =>
      {
        s.Summary = "Get a business by id for system admin";
        s.Description = "Retrieves a business by id for system admin.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok<BusinessSystemAdminResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetBusinessByIdRequest req, CancellationToken ct)
    {
      var business = await _dbContext.Businesses
        .Include(b => b.Municipality)
        .ThenInclude(m => m!.Province)
        .Include(b => b.User)
        .FirstOrDefaultAsync(b => b.Id == req.Id, ct);

      if (business is null)
        return TypedResults.NotFound();

      var mapper = new BusinessMapper();
      var response = mapper.FromEntity(business);

      if (!string.IsNullOrEmpty(business.Logo))
        response.Logo = await _blobService.PresignedGetUrl(business.Logo, ct);

      if (!string.IsNullOrEmpty(business.Banner))
        response.Banner = await _blobService.PresignedGetUrl(business.Banner, ct);

      return TypedResults.Ok(response);
    }
  }
}
