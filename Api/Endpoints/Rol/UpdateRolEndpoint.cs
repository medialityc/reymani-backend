using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol;

public sealed class UpdateRolEndpoint : Endpoint<UpdateRolRequest>
{
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;
  public UpdateRolEndpoint(IRolService rolService, IAuthorizationService authorizationService)
  {
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/rol/{RolId:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Rol";
      s.Description = "Actualiza un rol";
      s.ExampleRequest = new UpdateRolRequest
      {
        RolId = Guid.NewGuid(),
        Rol = new RolDto
        {
          Id = Guid.NewGuid(),
          Nombre = "Rol de Prueba",
          Descripcion = "Rol de Prueba"
        }
      };
    });
  }

  public override async Task HandleAsync(UpdateRolRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Rol"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var rol = await _rolService.GetByIdAsync(req.RolId);
    if (rol == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.Rol.Id != req.RolId)
    {
      AddError(r => r.RolId, "El ID del rol no coincide con el ID de la URL");
    }

    ThrowIfAnyErrors();

    if (rol != null)
    {
      rol.Nombre = req.Rol.Nombre;
      rol.Descripcion = req.Rol.Descripcion;
      await _rolService.UpdateAsync(rol);
      await SendOkAsync(ct);
    }
  }
}