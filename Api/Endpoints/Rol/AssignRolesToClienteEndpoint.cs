namespace reymani_web_api.Api.Endpoints.Rol;

public class AssignRolesToClienteEndpoint : Endpoint<AssignRolesToClienteRequest>
{
  private readonly IClienteService _clienteService;
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;

  public AssignRolesToClienteEndpoint(IClienteService clienteService, IRolService rolService, IAuthorizationService authorizationService)
  {
    _clienteService = clienteService;
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/rol/assign_roles_to_cliente");
    Summary(s =>
    {
      s.Summary = "Asignar Roles a Cliente";
      s.Description = "Asigna una lista de Roles a un Cliente";
      s.ExampleRequest = new AssignRolesToClienteRequest
      {
        ClienteId = Guid.NewGuid(),
        RoleIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
      };
    });
  }

  public override async Task HandleAsync(AssignRolesToClienteRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Asignar_Roles_A_Cliente"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var cliente = await _clienteService.GetClienteByIdAsync(req.ClienteId);
    if (cliente == null)
    {
      AddError(r => r.ClienteId, "Cliente no encontrado");
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
    await _clienteService.AssignRolesToClienteAsync(req.ClienteId, req.RoleIds);
    await SendOkAsync(ct);
  }
}
