using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

using static FastEndpoints.Ep;

namespace reymani_web_api.Endpoints.AddressUser;

public class GetAllCustomersEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<UserAddressResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetAllCustomersEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/userAddress");
    Summary(s =>
    {
      s.Summary = "Get all active customers";
      s.Description = "Retrieves a list of all customers.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok<IEnumerable<UserAddressResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new UserAddressMapper();

    var users = await _dbContext.UserAddresses
      .Where(p => p.IsActive == true)
      .AsNoTracking()
      .Include(p => p.Municipality)
      .Include(p => p.Municipality.Province)
      .AsNoTracking()
      .OrderBy(u => u.Id)
      .ToListAsync(ct);


    var response = users.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(response.AsEnumerable());
  }
}
