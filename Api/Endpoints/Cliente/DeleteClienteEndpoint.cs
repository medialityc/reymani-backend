using System;

namespace reymani_web_api.Api.Endpoints.Cliente;

public sealed class DeleteClienteEndpoint : Endpoint<DeleteClienteRequest>
{
  private readonly IClienteService _clienteService;

  private readonly IAuthorizationService _authorizationService;
  public DeleteClienteEndpoint(IClienteService clienteService, IAuthorizationService authorizationService)
  {
    _clienteService = clienteService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/cliente/{IdCliente:guid}");
    Summary(s =>
    {
      s.Summary = "Eliminar Cliente";
      s.Description = "Elimina un cliente de la base de datos";
      s.ExampleRequest = new DeleteClienteRequest
      {
        IdCliente = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(DeleteClienteRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Cliente"))
    {
      await SendUnauthorizedAsync(ct);
    }


    var cliente = await _clienteService.GetClienteByIdAsync(req.IdCliente);
    if (cliente == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _clienteService.DeleteClienteAsync(req.IdCliente);
    await SendOkAsync(ct);

  }
}