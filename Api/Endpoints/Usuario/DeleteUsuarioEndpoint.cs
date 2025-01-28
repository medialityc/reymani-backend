using System;

namespace reymani_web_api.Api.Endpoints.Usuario;

public sealed class DeleteUsuarioEndpoint : Endpoint<DeleteUsuarioRequest>
{
  private readonly IUsuarioService _UsuarioService;

  private readonly IAuthorizationService _authorizationService;
  public DeleteUsuarioEndpoint(IUsuarioService UsuarioService, IAuthorizationService authorizationService)
  {
    _UsuarioService = UsuarioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/usuario/{IdUsuario:guid}");
    Summary(s =>
    {
      s.Summary = "Eliminar Usuario";
      s.Description = "Elimina un Usuario de la base de datos";
      s.ExampleRequest = new DeleteUsuarioRequest
      {
        IdUsuario = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(DeleteUsuarioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Usuario"))
    {
      await SendUnauthorizedAsync(ct);
    }


    var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.IdUsuario);
    if (Usuario == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _UsuarioService.DeleteUsuarioAsync(req.IdUsuario);
    await SendOkAsync(ct);

  }
}