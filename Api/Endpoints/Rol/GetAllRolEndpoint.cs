using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol;

public sealed class GetAllRolEndpoint : EndpointWithoutRequest<GetAllRolResponse>
{
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllRolEndpoint(IRolService rolService, IAuthorizationService authorizationService)
  {
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/rol");
    Summary(s =>
    {
      s.Summary = "Obtener todos los roles";
      s.Description = "Obtiene todos los roles de la base de datos";
      s.ResponseExamples[200] = new GetAllRolResponse
      {
        Roles = new List<RolDto>
        {
          new RolDto
          {
            IdRol = Guid.NewGuid(),
            Nombre = "Administrador",
            Descripcion = "Rol con todos los permisos"
          }
        }
      };
    });
  }


  public override async Task<GetAllRolResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Roles"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var roles = await _rolService.GetAllAsync();
    return new GetAllRolResponse
    {
      Roles = roles.Select(r => new RolDto
      {
        IdRol = r.IdRol,
        Nombre = r.Nombre,
        Descripcion = r.Descripcion
      }).ToList()
    };

  }
}
