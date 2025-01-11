namespace reymani_web_api.Api.Endpoints.NegocioCliente
{
  public sealed class DeleteNegocioClienteEndpoint : Endpoint<DeleteNegocioClienteRequest>
  {
    private readonly INegocioClienteService _negocioClienteService;
    private readonly IAuthorizationService _authorizationService;
    private readonly INegocioService _negocioService;
    private readonly IClienteService _clienteService;

    public DeleteNegocioClienteEndpoint(INegocioClienteService negocioClienteService, IAuthorizationService authorizationService, INegocioService negocioService, IClienteService clienteService)
    {
      _negocioClienteService = negocioClienteService;
      _authorizationService = authorizationService;
      _clienteService = clienteService;
      _negocioService = negocioService;
    }

    public override void Configure()
    {
      Verbs(Http.DELETE);
      Routes("/negocio-cliente/{ClienteId}/{NegocioId}");
      Summary(s =>
      {
        s.Summary = "Eliminar NegocioCliente";
        s.Description = "Elimina un Negocio-Cliente. Es decir, se elimina la suscripcion del cliente a dicho negocio.";
        s.ExampleRequest = new DeleteNegocioClienteRequest
        {
          ClienteId = Guid.NewGuid(),
          NegocioId = Guid.NewGuid()
        };
      });
    }

    public override async Task HandleAsync(DeleteNegocioClienteRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Negocio_Cliente"))
      {
        await SendUnauthorizedAsync(ct);
        return;
      }

      var negocio = await _negocioService.GetByIdAsync(req.NegocioId);
      if (negocio == null)
      {
        AddError(req => req.NegocioId, "Negocio no encontrado");
      }

      var cliente = await _clienteService.GetClienteByIdAsync(req.ClienteId);
      if (cliente == null)
      {
        AddError(req => req.ClienteId, "Cliente no encontrado");
      }

      var negocioCliente = await _negocioClienteService.GetByIdClienteAndIdNegocio(req.ClienteId, req.NegocioId);
      if (negocioCliente == null)
      {
        AddError("El cliente no se encuentra subscrito al negocio.");
      }

      ThrowIfAnyErrors();

      await _negocioClienteService.DeleteAsync(req.ClienteId, req.NegocioId);
      await SendOkAsync(ct);
    }
  }
}
