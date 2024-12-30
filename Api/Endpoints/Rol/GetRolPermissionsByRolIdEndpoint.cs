using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public sealed class GetRolPermissionsByRolIdEndpoint : Endpoint<GetRolPermissionsByRolIdRequest, GetRolPermissionsByRolIdResponse>
{
  private readonly IRolService _rolService;

  public GetRolPermissionsByRolIdEndpoint(IRolService rolService)
  {
    _rolService = rolService;
  }
  public override void Configure()
  {
    Get("/rol/get-rol-permissions");
  }

  public override async Task<GetRolPermissionsByRolIdResponse> ExecuteAsync(GetRolPermissionsByRolIdRequest req, CancellationToken ct)
  {
    var rol = await _rolService.GetByIdAsync(req.RolId);
    if (rol == null)
    {
      await SendNotFoundAsync(ct);
    }

    var permisoIds = await _rolService.GetIdPermisosRolAsync(req.RolId);
    System.Console.WriteLine(permisoIds);
    return new GetRolPermissionsByRolIdResponse
    {
      PermisoIds = permisoIds.ToList()
    };
  }
}
