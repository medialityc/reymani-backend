using reymani_web_api.Data;
using FastEndpoints;
using reymani_web_api.Endpoints.Users.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Endpoints.Provinces;

public class GetProvinceByIdEndpoint : Endpoint<GetProvinceByIdRequest, Results<Ok<ProvinceResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetProvinceByIdEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/provinces/{id}");
    Summary(s =>
    {
      s.Summary = "Get province by Id";
      s.Description = "Retrieves details of a province by their ID.";
    });
    AllowAnonymous();
  }

  public override async Task<Results<Ok<ProvinceResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetProvinceByIdRequest req, CancellationToken ct)
  {
    var province = await _dbContext.Provinces
        .Include(p => p.Municipalities)
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (province is null)
      return TypedResults.NotFound();

    var mapper = new ProvinceMapper();

    var response = mapper.FromEntity(province);

    return TypedResults.Ok(response);
  }
}
