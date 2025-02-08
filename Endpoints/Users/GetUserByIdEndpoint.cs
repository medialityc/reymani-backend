using reymani_web_api.Endpoints.Users.Responses;
using reymani_web_api.Data;
using FastEndpoints;
using reymani_web_api.Endpoints.Users.Requests;
using Microsoft.AspNetCore.Http.HttpResults;

namespace reymani_web_api.Endpoints.Users;

public class GetUserByIdEndpoint : Endpoint<GetUserByIdRequest, Results<Ok<UserResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public GetUserByIdEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/users/{id}");
    Summary(s =>
    {
      s.Summary = "Get user by Id";
      s.Description = "Retrieves details of a user by their ID.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<UserResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetUserByIdRequest req, CancellationToken ct)
  {
    var user = await _dbContext.Users.FindAsync(req.Id, ct);

    if (user is null)
      return TypedResults.NotFound();

    return TypedResults.Ok(new UserResponse
    {
      Id = user.Id,
      ProfilePicture = user.ProfilePicture,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Email = user.Email,
      Phone = user.Phone,
      IsActive = user.IsActive,
      Role = user.Role,
      IsConfirmed = user.IsConfirmed
    });
  }
}
