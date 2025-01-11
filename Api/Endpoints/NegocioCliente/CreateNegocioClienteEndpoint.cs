namespace reymani_web_api.Api.Endpoints.NegocioCliente;

public sealed class CreateNegocioClienteEndpoint : Endpoint<CreateNegocioClienteRequest>
{
  private readonly INegocioClienteService _negocioClienteService;
  private readonly IAuthorizationService _authorizationService;
  private readonly INegocioService _negocioService;
  private readonly IClienteService _clienteService;

  public CreateNegocioClienteEndpoint(INegocioClienteService negocioClienteService, IAuthorizationService authorizationService, INegocioService negocioService, IClienteService clienteService)
  {
    _negocioClienteService = negocioClienteService;
    _authorizationService = authorizationService;
    _negocioService = negocioService;
    _clienteService = clienteService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/negocio-cliente");
    Summary(s =>
    {
      s.Summary = "Crear Negocio-Cliente";
      s.Description = "Crea una nueva relación entre negocio y cliente. Es decir, subscribir cliente a un negocio dado.";
      s.ExampleRequest = new CreateNegocioClienteRequest
      {
        IdCliente = Guid.NewGuid(),
        IdNegocio = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(CreateNegocioClienteRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Negocio_Cliente"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      AddError(req => req.IdNegocio, "Negocio no encontrado");
    }

    var cliente = await _clienteService.GetClienteByIdAsync(req.IdCliente);
    if (cliente == null)
    {
      AddError(req => req.IdCliente, "Cliente no encontrado");
    }

    var negocioCliente = new Domain.Entities.NegocioCliente
    {
      IdNegocioCliente = Guid.NewGuid(),
      IdCliente = req.IdCliente,
      IdNegocio = req.IdNegocio
    };

    await _negocioClienteService.AddAsync(negocioCliente);

    await SendOkAsync(negocioCliente, ct);
  }
}
