using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Requests;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.AddressUser;

public class GetByIdCustomerEndpoint : Endpoint<GetByIdCustomerRequest, Results<Ok<UserAddressResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetByIdCustomerEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/userAddress/{id}");
    Summary(s =>
    {
      s.Summary = "Get userAddress by Id";
      s.Description = "Retrieves details of a userAddress by their ID.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok<UserAddressResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetByIdCustomerRequest req, CancellationToken ct)
  {
    var userAddress = await _dbContext.UserAddresses
        .Where(p=> p.IsActive == true)
        .Include(p => p.Municipality)
        .Include(p => p.Municipality.Province)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

    if (userAddress is null)
      return TypedResults.NotFound();


    var mapper = new UserAddressMapper();

    var response = mapper.FromEntity(userAddress);

    return TypedResults.Ok(response);
  }
}
