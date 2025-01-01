using System;
using System.Threading;
using System.Threading.Tasks;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class ChangeClienteStatus : Endpoint<ChangeClienteStatusRequest>
{
  private readonly IClienteService _clienteService;
  private readonly IAuthorizationService _authorizationService;

  public ChangeClienteStatus(IClienteService clienteService, IAuthorizationService authorizationService)
  {
    _clienteService = clienteService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/cliente/{IdCliente:guid}/status");
    Summary(s =>
    {
      s.Summary = "Habilitar o deshabilitar Cliente";
      s.Description = "Cambia el estado de un cliente en la base de datos";
    });
  }

  public override async Task HandleAsync(ChangeClienteStatusRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Cambiar_Estado_Cliente"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var cliente = await _clienteService.GetClienteByIdAsync(req.IdCliente);
    if (cliente == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    cliente.Activo = req.Activo;
    await _clienteService.UpdateClienteAsync(cliente);
    await SendOkAsync(ct);
  }
}
