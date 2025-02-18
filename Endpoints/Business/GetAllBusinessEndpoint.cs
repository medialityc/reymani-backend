using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Business
{
  public class GetAllBusinessEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<SimpleBusinessResponse>>, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetAllBusinessEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/business");
      Summary(s =>
      {
        s.Summary = "Get all active businesses";
        s.Description = "Retrieves a list of active businesses.";
      });
      AllowAnonymous();
    }

    public override async Task<Results<Ok<IEnumerable<SimpleBusinessResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      var businesses = _dbContext.Businesses
        .Include(b => b.Municipality)
        .ThenInclude(m => m!.Province)
        .Where(b => b.IsActive)
        .OrderBy(b => b.Id)
        .AsNoTracking()
        .AsEnumerable();

      var mapper = new BusinessMapper();
      var response = await Task.WhenAll(businesses.Select(async b =>
      {
        var res = mapper.ToSimpleBusinessResponse(b);

        if (!string.IsNullOrEmpty(b.Logo))
          res.Logo = await _blobService.PresignedGetUrl(b.Logo, ct);

        if (!string.IsNullOrEmpty(b.Banner))
          res.Banner = await _blobService.PresignedGetUrl(b.Banner, ct);

        return res;
      }));

      return TypedResults.Ok(response.AsEnumerable());
    }
  }
}