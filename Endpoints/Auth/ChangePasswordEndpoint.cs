using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Auth.Requests;


namespace reymani_web_api.Endpoints.Auth
{
  public class ChangePasswordEndpoint : Endpoint<ChangePasswordRequest, Results<Ok<string>, UnauthorizedHttpResult, NotFound>>
  {
    private readonly AppDbContext _dbContext;

    public ChangePasswordEndpoint(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public override void Configure()
    {
      Post("/auth/change-password");
      Summary(s =>
      {
        s.Summary = "Change password";
        s.Description = "Change the password of the current user";
      });
    }

    public override async Task<Results<Ok<string>, UnauthorizedHttpResult, NotFound>> ExecuteAsync(ChangePasswordRequest request, CancellationToken ct)
    {
      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        return TypedResults.Unauthorized();

      var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
      if (user is null)
        return TypedResults.NotFound();

      if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password))
        AddError(o => o.CurrentPassword, "Invalid password");

      ThrowIfAnyErrors();

      user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

      var result = await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok("Password changed successfully");
    }
  }
}
