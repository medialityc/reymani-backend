using System;
using reymani_web_api.Application.Services;

namespace reymani_web_api.Api.Endpoints.Rol;

public class AssingPermisosToRolEndpoint : Endpoint<AssingPermisosToRolRequest>
{
  private readonly IRolService _rolService;

  public AssingPermisosToRolEndpoint(IRolService rolService)
  {
    _rolService = rolService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/roles/assign_permissions_to_role");
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
    await _rolService.AssignPermissionsAsync(req.RolId, req.PermisoIds);
    await SendOkAsync(ct);
  }
}
