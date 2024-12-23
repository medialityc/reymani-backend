using System;
using reymani_web_api.Application.Services;

namespace reymani_web_api.Api.Endpoints.Rol;

public class AssingPermisosToRolEndpoint : Endpoint<AssingPermisosToRolRequest>
{
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;

  public AssingPermisosToRolEndpoint(IRolService rolService, IAuthorizationService authorizationService)
  {
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/rol/assign_permissions_to_role");
    Summary(s =>
    {
      s.Summary = "Asignar Permisos a Rol";
      s.Description = "Asigna permisos a un rol";
      s.ExampleRequest = new AssingPermisosToRolRequest
      {
        RolId = Guid.NewGuid(),
        PermisoIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
      };
    });
  }

  public override async Task HandleAsync(AssingPermisosToRolRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Asignar_Permisos_A_Rol"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var rol = await _rolService.GetByIdAsync(req.RolId);
    if (rol == null)
    {
      AddError(r => r.RolId, "Rol no encontrado");
    }

    ThrowIfAnyErrors();
    await _rolService.AssignPermissionsAsync(req.RolId, req.PermisoIds);
    await SendOkAsync(ct);
  }
}
