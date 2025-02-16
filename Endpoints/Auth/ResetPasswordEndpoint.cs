using System;

using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Auth.Requests;
using reymani_web_api.Services.EmailServices;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Auth
{
  public class ResetPasswordEndpoint : Endpoint<ResetPasswordRequest, Results<Ok<string>, NotFound>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IEmailSender _emailSender;

    public ResetPasswordEndpoint(AppDbContext dbContext, IEmailSender emailSender)
    {
      _dbContext = dbContext;
      _emailSender = emailSender;
    }

    public override void Configure()
    {
      Post("/auth/reset-password");
      AllowAnonymous();
      Summary(s =>
      {
        s.Summary = "Reset Password";
        s.Description = "Resets the password using a 4-digit confirmation code";
      });
    }

    public override async Task<Results<Ok<string>, NotFound>> ExecuteAsync(ResetPasswordRequest request, CancellationToken ct)
    {
      var oneDayAgo = DateTimeOffset.UtcNow.AddDays(-1);
      
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == request.Email , ct);
      if (user == null)
        return TypedResults.NotFound();
      
      var confirmation = await _dbContext
        .ForgotPasswordNumbers
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.UserId == user.Id && c.Number == request.ConfirmationCode && c.UpdatedAt >= oneDayAgo, ct);
      if (confirmation == null)
          return TypedResults.NotFound();

      // Actualizar la contraseña
      user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
      _dbContext.Users.Update(user);

      // Eliminar el registro de confirmation
      _dbContext.Set<ForgotPasswordNumber>().Remove(confirmation);

      await _dbContext.SaveChangesAsync(ct);

      return TypedResults.Ok("Password reset successfully.");
    }
  }
}
