namespace reymani_web_api.Api.Endpoints.NegocioCliente;

public sealed class CreateNegocioClienteEndpoint : Endpoint<CreateNegocioClienteRequest>
{
  private readonly INegocioClienteService _negocioClienteService;
  private readonly IAuthorizationService _authorizationService;

  public CreateNegocioClienteEndpoint(INegocioClienteService negocioClienteService, IAuthorizationService authorizationService)
  {
    _negocioClienteService = negocioClienteService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/negocio-cliente");
    Summary(s =>
    {
      s.Summary = "Crear Negocio-Cliente";
      s.Description = "Crea una nueva relación entre negocio y cliente.";
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
