using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol;

public sealed class GetRolPermissionsByRolIdEndpoint : Endpoint<GetRolPermissionsByRolIdRequest, GetRolPermissionsByRolIdResponse>
{
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;


  public GetRolPermissionsByRolIdEndpoint(IRolService rolService, IAuthorizationService authorizationService)
  {
    _rolService = rolService;
    _authorizationService = authorizationService;
  }
  public override void Configure()
  {
    Get("/rol/get-rol-permissions");
    Summary(s =>
    {
      s.Summary = "Obtener permisos de un rol";
      s.Description = "Obtiene los permisos de un rol";
      s.ExampleRequest = new GetRolPermissionsByRolIdRequest
      {
        RolId = Guid.NewGuid()
      };
      s.ResponseExamples[200] = new GetRolPermissionsByRolIdResponse
      {
        Permisos = new List<PermisoDto>
        {
          new() {
            Id = Guid.NewGuid(),
            Codigo = "Ver_Prueba",
            Descripcion = "Permiso de Prueba"
          }
        }
      };
    });
  }

  public override async Task<GetRolPermissionsByRolIdResponse> ExecuteAsync(GetRolPermissionsByRolIdRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Permisos_Rol"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var rol = await _rolService.GetByIdAsync(req.RolId);
    if (rol == null)
    {
      await SendNotFoundAsync(ct);
    }

    var permisos = await _rolService.GetPermisosRolAsync(req.RolId);
    var permisoDtos = permisos.Select(p => new PermisoDto
    {
      Id = p.IdPermiso,
      Codigo = p.Codigo,
      Descripcion = p.Descripcion
    }).ToList();
    return new GetRolPermissionsByRolIdResponse
    {
      Permisos = permisoDtos
    };
  }
}
