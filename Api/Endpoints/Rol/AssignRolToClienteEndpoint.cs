
namespace reymani_web_api.Api.Endpoints.Rol;

public class AssignRolToClienteEndpoint : Endpoint<AssingRoleToClienteRequest>
{
  private readonly IClienteService _clienteService;
  private readonly IRolService _rolService;
  private readonly IAuthorizationService _authorizationService;

  public AssignRolToClienteEndpoint(IClienteService clienteService, IRolService rolService, IAuthorizationService authorizationService)
  {
    _clienteService = clienteService;
    _rolService = rolService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/rol/assign_role_to_client");
    Summary(s =>
    {
      s.Summary = "Asignar Rol a Cliente";
      s.Description = "Asigna un Rol a un Cliente";
      s.ExampleRequest = new AssingRoleToClienteRequest
      {
        ClienteId = Guid.NewGuid(),
        RoleId = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(AssingRoleToClienteRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Asignar_Rol_A_Cliente"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var cliente = await _clienteService.GetClienteByIdAsync(req.ClienteId);
    if (cliente == null)
    {
      AddError(r => r.ClienteId, "Cliente no encontrado");
    }

    var rol = await _rolService.GetByIdAsync(req.RoleId);
    if (rol == null)
    {
      AddError(r => r.RoleId, "Rol no encontrado");
    }

    ThrowIfAnyErrors();
    await _clienteService.AssignRoleToClienteAsync(req.ClienteId, req.RoleId);
    await SendOkAsync(ct);
  }
}
