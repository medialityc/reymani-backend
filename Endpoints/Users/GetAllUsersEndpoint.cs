using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Responses;

namespace reymani_web_api.Endpoints.Users
{
  public class GetAllUsersEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<UserResponse>>, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;

    public GetAllUsersEndpoint(AppDbContext dbContext) => _dbContext = dbContext;

    public override void Configure()
    {
      Get("/users");
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok<IEnumerable<UserResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      var users = _dbContext.Users.OrderBy(u => u.Id).AsEnumerable();
      var response = users.Select(u => new UserResponse
      {
        Id = u.Id,
        ProfilePicture = u.ProfilePicture,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Email = u.Email,
        Phone = u.Phone,
        IsActive = u.IsActive,
        Role = u.Role,
        IsConfirmed = u.IsConfirmed
      });
      return TypedResults.Ok(response);
    }
  }
}
