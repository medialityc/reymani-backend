using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Responses;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.Mappers;
using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Endpoints.Users
{
  public class GetAllUsersEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<UserResponse>>, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetAllUsersEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/users");
      Summary(s =>
      {
        s.Summary = "Get all users";
        s.Description = "Retrieves a list of all users.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok<IEnumerable<UserResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      var mapper = new UserMapper();
      var users = _dbContext.Users.OrderBy(u => u.Id).AsNoTracking().AsEnumerable();
      var response = await Task.WhenAll(users.Select(async u =>
      {
        var resp = mapper.FromEntity(u);
        if (!string.IsNullOrEmpty(u.ProfilePicture))
          resp.ProfilePicture = await _blobService.PresignedGetUrl(u.ProfilePicture, ct);
        return resp;
      }));
      return TypedResults.Ok(response.AsEnumerable());
    }
  }
}
