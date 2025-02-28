using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Users
{
  public class DeleteUserEndpoint : Endpoint<GetUserByIdRequest, Results<Ok, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteUserEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Delete("/users/{id}");
      Summary(s =>
      {
        s.Summary = "Delete user";
        s.Description = "Deletes an existing user by ID.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(GetUserByIdRequest req, CancellationToken ct)
    {
      var user = await _dbContext.Users.FindAsync(req.Id, ct);

      if (user is null)
        return TypedResults.NotFound();

      _dbContext.Users.Remove(user);
      await _dbContext.SaveChangesAsync(ct);

      if (!string.IsNullOrEmpty(user.ProfilePicture))
        await _blobService.DeleteObject(user.ProfilePicture, ct);

      return TypedResults.Ok();
    }
  }
}