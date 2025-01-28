using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Permiso;

public class GetPermisoByIdEndpoint : Endpoint<GetPermisoByIdRequest, PermisoDto>
{
  private readonly IPermisoService _permisoService;

  private readonly IAuthorizationService _authorizationService;

  public GetPermisoByIdEndpoint(IPermisoService permisoService, IAuthorizationService authorizationService)
  {
    _permisoService = permisoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.GET);
    Routes("/permiso/{PermisoId:guid}");
    Summary(s =>
    {
      s.Summary = "Obtener permiso por ID";
      s.Description = "Obtiene un permiso de la base de datos por su ID";
      s.ExampleRequest = new GetPermisoByIdRequest
      {
        PermisoId = Guid.NewGuid()
      };
      s.ResponseExamples[200] = new PermisoDto
      {
        Id = Guid.NewGuid(),
        Codigo = "Crear_Usuario",
        Descripcion = "Permiso para crear Usuario"
      };
      s.Responses[404] = "Permiso no encontrado";
      s.Responses[200] = "Permiso encontrado";
    });
  }

  public override async Task HandleAsync(GetPermisoByIdRequest request, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Permiso"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var permiso = await _permisoService.GetByIdAsync(request.PermisoId);

    if (permiso is null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    var permisoDto = new PermisoDto
    {
      Id = permiso.IdPermiso,
      Codigo = permiso.Codigo,
      Descripcion = permiso.Descripcion ?? string.Empty
    };
    await SendOkAsync(permisoDto, ct);
  }

}
