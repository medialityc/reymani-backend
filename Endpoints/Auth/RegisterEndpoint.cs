using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Auth.Requests;
using reymani_web_api.Endpoints.Auth.Responses;
using reymani_web_api.Utils.Tokens;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Auth
{
  public class RegisterEndpoint : Endpoint<RegisterRequest, Results<Ok<RegisterResponse>, Conflict>>
  {
    private readonly AppDbContext _dbContext;

    public RegisterEndpoint(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public override void Configure()
    {
      Post("/auth/register");
      AllowAnonymous();
      AllowFormData();
    }

    public override async Task<Results<Ok<RegisterResponse>, Conflict>> ExecuteAsync(RegisterRequest request, CancellationToken ct)
    {
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == request.Email);
      if (user != null)
        return TypedResults.Conflict();

      user = new User
      {
        Email = request.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        FirstName = request.FirstName,
        LastName = request.LastName,
        Phone = request.Phone,
        ProfilePicture = "", //Falta poner la url de la imagen cuando se suba a minio
        IsActive = true,
        Role = UserRole.Customer
      };

      _dbContext.Users.Add(user);
      await _dbContext.SaveChangesAsync();

      var token = TokenGenerator.GenerateToken(user);
      return TypedResults.Ok(new RegisterResponse { Token = token });
    }
  }
}
