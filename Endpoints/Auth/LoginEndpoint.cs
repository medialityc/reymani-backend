using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Auth.Requests;
using reymani_web_api.Endpoints.Auth.Responses;
using reymani_web_api.Utils.Tokens;

namespace reymani_web_api.Endpoints.Auth
{
  public class LoginEndpoint : Endpoint<LoginRequest, Results<Ok<LoginResponse>, UnauthorizedHttpResult>>
  {
    private readonly AppDbContext _dbContext;
    private readonly TokenGenerator _tokenGenerator;

    public LoginEndpoint(AppDbContext dbContext, TokenGenerator tokenGenerator)
    {
      _dbContext = dbContext;
      _tokenGenerator = tokenGenerator;
    }

    public override void Configure()
    {
      Post("/auth/login");
      AllowAnonymous();
      Summary(s =>
      {
        s.Summary = "Login";
        s.Description = "Login to the application";
      });
    }

    public override async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult>> ExecuteAsync(LoginRequest request, CancellationToken ct)
    {
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == request.Email, ct);
      if (user is null || !user.IsActive || !user.IsConfirmed || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        return TypedResults.Unauthorized();

      var token = _tokenGenerator.GenerateToken(user);
      return TypedResults.Ok(new LoginResponse { Token = token });
    }
  }
}
