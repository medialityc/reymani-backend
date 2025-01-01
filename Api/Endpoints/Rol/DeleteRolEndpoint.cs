using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public sealed class DeleteRolEndpoint : Endpoint<DeleteRolRequest>
{
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;

  public DeleteRolEndpoint(IRolService rolService, IAuthorizationService authorizationService)
  {
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/rol/{RolId}");
    Summary(s =>
    {
      s.Summary = "Eliminar Rol";
      s.Description = "Elimina un Rol";
      s.ExampleRequest = new DeleteRolRequest
      {
        RolId = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(DeleteRolRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Rol"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var rol = await _rolService.GetByIdAsync(req.RolId);
    if (rol == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _rolService.DeleteAsync(req.RolId);
    await SendOkAsync(ct);
  }
}