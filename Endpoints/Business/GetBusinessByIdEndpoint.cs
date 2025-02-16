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
  public class GetBusinessByIdEndpoint : Endpoint<GetBusinessByIdRequest, Results<Ok<BusinessResponse>, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetBusinessByIdEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/business/{Id}");
      AllowAnonymous();
      Summary(s =>
      {
        s.Summary = "Get an active business by id";
        s.Description = "Retrieves an active business by id.";
      });
    }

    public override async Task<Results<Ok<BusinessResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetBusinessByIdRequest req, CancellationToken ct)
    {
      var business = await _dbContext.Businesses
        .Where(b => b.IsActive)
        .Include(b => b.Municipality)
        .ThenInclude(m => m!.Province)
        .FirstOrDefaultAsync(b => b.Id == req.Id, ct);

      if (business is null)
        return TypedResults.NotFound();

      var mapper = new BusinessMapper();
      var response = mapper.ToBusinessResponse(business);

      if (!string.IsNullOrEmpty(business.Logo))
        response.Logo = await _blobService.PresignedGetUrl(business.Logo, ct);

      if (!string.IsNullOrEmpty(business.Banner))
        response.Banner = await _blobService.PresignedGetUrl(business.Banner, ct);

      return TypedResults.Ok(response);
    }
  }
}
