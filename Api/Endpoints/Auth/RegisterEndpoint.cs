using System;

namespace reymani_web_api.Api.Endpoints.Auth;

public class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
{
  private readonly IAuthService _authService;

  public RegisterEndpoint(IAuthService authService)
  {
    _authService = authService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/auth/register");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Registrar Cliente";
      s.Description = "Registra un nuevo cliente en la aplicación";
      s.ExampleRequest = new RegisterRequest
      {
        NumeroCarnet = "04112086258",
        Nombre = "John",
        Apellidos = "Doe Martin",
        Username = "johndoe",
        Password = "Jhondoe123"
      };
      s.ResponseExamples[200] = new RegisterResponse
      {
        Token = "fhusdyr723ryui23rh7891y43u1b4u12gbrfef13"
      };
    });
  }

  public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
  {
    try
    {
      var token = await _authService.RegisterAsync(req);
      await SendOkAsync(new RegisterResponse { Token = token }, ct);
    }
    catch (Exception ex)
    {
      AddError(ex.Message);
      ThrowIfAnyErrors();
    }
  }
}