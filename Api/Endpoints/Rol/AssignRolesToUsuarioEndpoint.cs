namespace reymani_web_api.Api.Endpoints.Rol;

public class AssignRolesToUsuarioEndpoint : Endpoint<AssignRolesToUsuarioRequest>
{
  private readonly IUsuarioService _UsuarioService;
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;

  public AssignRolesToUsuarioEndpoint(IUsuarioService UsuarioService, IRolService rolService, IAuthorizationService authorizationService)
  {
    _UsuarioService = UsuarioService;
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/rol/assign_roles_to_Usuario");
    Summary(s =>
    {
      s.Summary = "Asignar Roles a Usuario";
      s.Description = "Asigna una lista de Roles a un Usuario";
      s.ExampleRequest = new AssignRolesToUsuarioRequest
      {
        UsuarioId = Guid.NewGuid(),
        RoleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
      };
    });
  }

  public override async Task HandleAsync(AssignRolesToUsuarioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Asignar_Roles_A_Usuario"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.UsuarioId);
    if (Usuario == null)
    {
      AddError(r => r.UsuarioId, "Usuario no encontrado");
    }

    foreach (var roleId in req.RoleIds)
    {
      var rol = await _rolService.GetByIdAsync(roleId);
      if (rol == null)
      {
        AddError(r => r.RoleIds, $"Rol con ID {roleId} no encontrado");
      }
    }

    ThrowIfAnyErrors();
    await _UsuarioService.AssignRolesToUsuarioAsync(req.UsuarioId, req.RoleIds);
    await SendOkAsync(ct);
  }
}
