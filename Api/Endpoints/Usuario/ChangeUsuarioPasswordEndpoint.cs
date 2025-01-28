using System;

namespace reymani_web_api.Api.Endpoints.Usuario;

public sealed class ChangeUsuarioPasswordEndpoint : Endpoint<ChangeUsuarioPasswordRequest>
{
  private readonly IUsuarioService _UsuarioService;
  private readonly IAuthorizationService _authorizationService;

  public ChangeUsuarioPasswordEndpoint(IUsuarioService UsuarioService, IAuthorizationService authorizationService)
  {
    _UsuarioService = UsuarioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/usuario/change-password");
    Summary(s =>
      {
        s.Summary = "Cambiar contraseña del Usuario";
        s.Description = "Cambiar contraseña del Usuario en la base de datos";
        s.ExampleRequest = new ChangeUsuarioPasswordRequest
        {
          UsuarioId = new Guid(),
          NewPassword = "dasdsadsa",
          Password = "sdadsadsadsadsa"
        };
      });
  }

  public override async Task HandleAsync(ChangeUsuarioPasswordRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Cambiar_Contraseña"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.UsuarioId);
    if (Usuario == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (!await _UsuarioService.CheckPasswordAsync(Usuario, req.Password))
    {
      AddError(r => r.Password, "Contraseña incorrecta");
    }

    ThrowIfAnyErrors();

    await _UsuarioService.ChangePasswordAsync(Usuario, req.NewPassword);
    await SendOkAsync(ct);
  }
}