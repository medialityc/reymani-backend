using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Permiso;

public sealed class GetAllPermisoEndpoint : EndpointWithoutRequest<GetAllPermisoResponse>
{
  private readonly IPermisoService _permisoService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllPermisoEndpoint(IPermisoService permisoService, IAuthorizationService authorizationService)
  {
    _permisoService = permisoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/permiso");
    Summary(s =>
    {
      s.Summary = "Obtener todos los permisos";
      s.Description = "Obtiene todos los permisos de la base de datos";
      s.ResponseExamples[200] = new GetAllPermisoResponse
      {
        Permisos = new List<PermisoDto>
        {
          new PermisoDto
          {
            Id = Guid.NewGuid(),
            Codigo = "PERMISO",
            Descripcion = "Permiso de acceso"
          }
        }
      };
    });
  }

  public override async Task<GetAllPermisoResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Permisos"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var permisos = await _permisoService.GetAllAsync();
    return new GetAllPermisoResponse
    {
      Permisos = permisos.Select(p => new PermisoDto
      {
        Id = p.IdPermiso,
        Codigo = p.Codigo,
        Descripcion = p.Descripcion ?? string.Empty
      }).ToList()
    };

  }
}