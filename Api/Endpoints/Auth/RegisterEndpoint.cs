using System;

namespace reymani_web_api.Api.Endpoints.Auth;

public class RegisterEndpoint : Endpoint<RegisterRequest>
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
  }

  public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
  {
    if (await _authService.IsUsernameInUseAsync(req.Username))
    {
      AddError(r => r.Username, "Ya ese Nombre de Usuario está en uso por otro cliente");
    }

    if (await _authService.IsNumeroCarnetInUseAsync(req.NumeroCarnet))
    {
      AddError(r => r.NumeroCarnet, "Ya ese Número de Carnet está en uso por otro cliente");
    }


    ThrowIfAnyErrors();
    var token = await _authService.RegisterAsync(req);

    await SendOkAsync(new { token }, ct);

  }
}