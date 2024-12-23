using System;
using reymani_web_api.Application.DTOs;
using reymani_web_api.Application.Services;

namespace reymani_web_api.Api.Endpoints.Rol
{
  public class GetRolByIdEndpoint : Endpoint<GetRolByIdRequest, RolDto>
  {
    private readonly IRolService _rolService;
    private readonly IAuthorizationService _authorizationService;

    public GetRolByIdEndpoint(IRolService rolService, IAuthorizationService authorizationService)
    {
      _rolService = rolService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/rol/{RolId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener rol por ID";
        s.Description = "Obtiene un rol de la base de datos por su ID";
        s.ExampleRequest = new GetRolByIdRequest
        {
          RolId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new RolDto
        {
          IdRol = Guid.NewGuid(),
          Nombre = "Administrador",
          Descripcion = "Rol con todos los permisos"
        };
        s.Responses[404] = "Rol no encontrado";
        s.Responses[200] = "Rol encontrado";
      });
    }

    public override async Task HandleAsync(GetRolByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Rol"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var rol = await _rolService.GetByIdAsync(req.RolId);
      if (rol == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var rolDto = new RolDto
      {
        IdRol = rol.IdRol,
        Nombre = rol.Nombre,
        Descripcion = rol.Descripcion
      };

      await SendOkAsync(rolDto, ct);
    }
  }
}
