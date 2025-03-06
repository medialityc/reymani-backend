using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Mappers;

namespace reymani_web_api.Endpoints.AddressUser;

public class GetAllSystemAdminEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<UserAddressResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public GetAllSystemAdminEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/userAddress/admin");
    Summary(s =>
    {
      s.Summary = "Get all customers";
      s.Description = "Retrieves a list of all customers.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<IEnumerable<UserAddressResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
  {
    var mapper = new UserAddressMapper();

    var users = await _dbContext.UserAddresses
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
