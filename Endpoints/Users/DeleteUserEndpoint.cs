using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;

namespace reymani_web_api.Endpoints.Users
{
  public class DeleteUserEndpoint : Endpoint<GetUserByIdRequest, Results<Ok, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext dbContext;

    public DeleteUserEndpoint(AppDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    public override void Configure()
    {
      Delete("/users/{id}");
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(GetUserByIdRequest req, CancellationToken ct)
    {
      var user = await dbContext.Users.FindAsync(req.Id, ct);

      if (user is null)
        return TypedResults.NotFound();

      dbContext.Users.Remove(user);
      await dbContext.SaveChangesAsync(ct);

      return TypedResults.Ok();
    }
  }
}
