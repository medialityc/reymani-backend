using reymani_web_api.Endpoints.Users.Responses;
using reymani_web_api.Data;
using FastEndpoints;
using reymani_web_api.Endpoints.Users.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Utils.Mappers;

namespace reymani_web_api.Endpoints.Users;

public class GetUserByIdEndpoint : Endpoint<GetUserByIdRequest, Results<Ok<UserResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public GetUserByIdEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
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

    var mapper = new UserMapper();
    var response = mapper.FromEntity(user);
    if (!string.IsNullOrEmpty(user.ProfilePicture))
      response.ProfilePicture = await _blobService.PresignedGetUrl(user.ProfilePicture, ct);

    return TypedResults.Ok(response);
  }
}
