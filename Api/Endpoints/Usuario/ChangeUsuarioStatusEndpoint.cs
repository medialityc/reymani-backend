using System;
using System.Threading;
using System.Threading.Tasks;

namespace reymani_web_api.Api.Endpoints.Usuario;

public class ChangeUsuarioStatus : Endpoint<ChangeUsuarioStatusRequest>
{
  private readonly IUsuarioService _UsuarioService;
  private readonly IAuthorizationService _authorizationService;

  public ChangeUsuarioStatus(IUsuarioService UsuarioService, IAuthorizationService authorizationService)
  {
    _UsuarioService = UsuarioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/usuario/{IdUsuario:guid}/status");
    Summary(s =>
    {
      s.Summary = "Habilitar o deshabilitar Usuario";
      s.Description = "Cambia el estado de un Usuario en la base de datos";
    });
  }

  public override async Task HandleAsync(ChangeUsuarioStatusRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Cambiar_Estado_Usuario"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.IdUsuario);
    if (Usuario == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    Usuario.Activo = req.Activo;
    await _UsuarioService.UpdateUsuarioAsync(Usuario);
    await SendOkAsync(ct);
  }
}
