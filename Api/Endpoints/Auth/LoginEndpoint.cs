using System;

namespace reymani_web_api.Api.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginRequest>
{
  private readonly IAuthService _authService;

  public LoginEndpoint(IAuthService authService)
  {
    _authService = authService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/auth/login");
    AllowAnonymous();
  }

  public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
  {
    try
    {
      var token = await _authService.LoginAsync(req);
      await SendOkAsync(new { token }, ct);
    }
    catch (UnauthorizedAccessException)
    {
      AddError("Credenciales inválidas");
      ThrowIfAnyErrors();
    }
  }
}
