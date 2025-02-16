using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Auth.Requests;

namespace reymani_web_api.Endpoints.Auth
{
  public class ConfirmEndpoint : Endpoint<ConfirmEndpointRequest, Results<Ok<string>, UnauthorizedHttpResult, NotFound>>
  {
    private readonly AppDbContext _dbContext;

    public ConfirmEndpoint(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public override void Configure()
    {
      Post("/auth/confirm");
      AllowAnonymous();
      Summary(s =>
      {
        s.Summary = "Confirm user";
        s.Description = "Validates a 4-digit confirmation code for the user";
      });
    }

    public override async Task<Results<Ok<string>, UnauthorizedHttpResult, NotFound>> ExecuteAsync(ConfirmEndpointRequest request, CancellationToken ct)
    {
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == request.Email, ct);
      if (user == null)
        return TypedResults.NotFound();

      var confirmation = await _dbContext.ConfirmationNumbers.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == user.Id, ct);
      if (confirmation == null)
        return TypedResults.NotFound();

      if (confirmation.Number != request.ConfirmationCode)
        AddError(o => o.ConfirmationCode, "Invalid confirmation code");

      ThrowIfAnyErrors();

      _dbContext.ConfirmationNumbers.Remove(confirmation);
      user.IsConfirmed = true;
      await _dbContext.SaveChangesAsync(ct);

      return TypedResults.Ok("User confirmed successfully");
    }
  }
}
