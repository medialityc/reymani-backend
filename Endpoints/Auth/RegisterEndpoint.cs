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
  public class RegisterEndpoint : Endpoint<RegisterRequest, Results<Ok<string>, Conflict>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IEmailSender _emailSender; // Nueva inyección

    public RegisterEndpoint(AppDbContext dbContext, IEmailSender emailSender) // Constructor modificado
    {
      _dbContext = dbContext;
      _emailSender = emailSender;
    }

    public override void Configure()
    {
      Post("/auth/register");
      AllowAnonymous();
      AllowFormData();
      Summary(s =>
      {
        s.Summary = "Register";
        s.Description = "Register a new user";
      });
    }

    public override async Task<Results<Ok<string>, Conflict>> ExecuteAsync(RegisterRequest request, CancellationToken ct)
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
        Role = UserRole.Customer,
        IsConfirmed = false
      };

      _dbContext.Users.Add(user);
      await _dbContext.SaveChangesAsync();

      // Generar código de confirmación de 4 dígitos
      var rnd = new Random();
      int confirmationCode = rnd.Next(1000, 10000);

      // Enviar correo con el código de confirmación
      await _emailSender.SendEmailAsync(user.Email, "Confirm your account", $"Your confirmation code is: {confirmationCode}");

      return TypedResults.Ok("User registered successfully. Please check your email to confirm your account.");
    }
  }
}
