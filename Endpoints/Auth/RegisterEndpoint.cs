using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Auth.Requests;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Services.EmailServices;
using reymani_web_api.Utils.Validations;

using ReymaniWebApi.Data.Models;


namespace reymani_web_api.Endpoints.Auth
{
  public class RegisterEndpoint : Endpoint<RegisterRequest, Results<Ok<string>, Conflict>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IEmailSender _emailSender;
    private readonly IBlobService _blobService;
    private readonly IEmailTemplateService _emailTemplateService;

    public RegisterEndpoint(AppDbContext dbContext, IEmailSender emailSender, IBlobService blobService, IEmailTemplateService emailTemplateService)
    {
      _dbContext = dbContext;
      _emailSender = emailSender;
      _blobService = blobService;
      _emailTemplateService = emailTemplateService;
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
      var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == request.Email, ct);
      if (user != null)
        return TypedResults.Conflict();

      string objectPath = string.Empty;
      if (request.ProfilePicture != null)
      {
        string fileCode = $"{Guid.NewGuid()}";
        objectPath = await _blobService.UploadObject(request.ProfilePicture, fileCode, ct);
      }

      user = new User
      {
        Email = request.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        FirstName = request.FirstName,
        LastName = request.LastName,
        Phone = request.Phone,
        ProfilePicture = objectPath,
        IsActive = true,
        Role = UserRole.Customer,
        IsConfirmed = false
      };

      _dbContext.Users.Add(user);
      await _dbContext.SaveChangesAsync(ct);

      // Generar código de confirmación de 4 dígitos
      var rnd = new Random();
      int confirmationCode = rnd.Next(1000, 10000);

      // Guardar el código de confirmación en base de datos usando la entidad ConfirmationNumber
      var confirmation = new ConfirmationNumber
      {
        UserId = user.Id,
        Number = confirmationCode.ToString()
      };
      _dbContext.ConfirmationNumbers.Add(confirmation);
      await _dbContext.SaveChangesAsync(ct);

      var emailBody = await _emailTemplateService.GetTemplateAsync("ConfirmationEmail", new
      {
        confirmationCode
      });
      await _emailSender.SendEmailAsync(user.Email, "Confirm your account", emailBody);

      return TypedResults.Ok("User registered successfully. Please check your email to confirm your account.");
    }
  }
}
