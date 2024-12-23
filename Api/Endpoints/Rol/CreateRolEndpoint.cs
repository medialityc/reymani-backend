using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol;

public sealed class CreateRolEndpoint : Endpoint<CreateRolRequest>
{
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;

  public CreateRolEndpoint(IRolService rolService, IAuthorizationService authorizationService)
  {
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/rol");
    Summary(s =>
    {
      s.Summary = "Crear Rol";
      s.Description = "Crea un nuevo rol";
      s.ExampleRequest = new CreateRolRequest
      {
        Nombre = "Rol de Prueba",
        Descripcion = "Rol de Prueba"
      };
    });
  }

  public override async Task HandleAsync(CreateRolRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Rol"))
    {
      await SendUnauthorizedAsync(ct);
    }

    if (await _rolService.RolNameExistsAsync(req.Nombre))
    {
      AddError(r => r.Nombre, "El nombre del rol ya existe");
      ThrowIfAnyErrors();
    }

    var rol = new reymani_web_api.Domain.Entities.Rol
    {
      IdRol = Guid.NewGuid(),
      Nombre = req.Nombre,
      Descripcion = req.Descripcion
    };

    await _rolService.AddAsync(rol);

    await SendOkAsync(rol, ct);
  }
}