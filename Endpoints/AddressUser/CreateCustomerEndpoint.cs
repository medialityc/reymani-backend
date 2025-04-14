using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Requests;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Mappers;


namespace reymani_web_api.Endpoints.AddressUser;

public class CreateCustomerEndpoint : Endpoint<CreateCustomerRequest, Results<Created<UserAddressResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public CreateCustomerEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Post("/userAddress");
    Summary(s =>
    {
      s.Summary = "Create userAddress";
      s.Description = "Creates a new user address.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Created<UserAddressResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateCustomerRequest req, CancellationToken ct)
  {
    var existingAddressName = await _dbContext.UserAddresses.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(req.Name.ToLower()) && x.IsActive, ct);
    if (existingAddressName != null)
    {
      return TypedResults.Conflict();
    }
    
    var userIdClaim = User.Claims.First(c => c.Type == "Id");
    int userId = int.Parse(userIdClaim.Value);

    var mapper = new UserAddressMapper();
    var userAddress = mapper.ToEntity(req, userId);

    // Agrega la nueva direccion a la base de datos
    _dbContext.UserAddresses.Add(userAddress);
    await _dbContext.SaveChangesAsync(ct);


    var response = mapper.FromEntity(userAddress);


    return TypedResults.Created($"/userAddress/{userAddress.Id}", response);
  }
}
