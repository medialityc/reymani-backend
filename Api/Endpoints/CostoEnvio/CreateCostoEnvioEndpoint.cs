namespace reymani_web_api.Api.Endpoints.CostoEnvio;

public sealed class CreateCostoEnvioEndpoint : Endpoint<CreateCostoEnvioRequest>
{
  private readonly ICostoEnvioService _costoEnvioService;
  private readonly IAuthorizationService _authorizationService;
  private readonly INegocioService _negocioService;

  public CreateCostoEnvioEndpoint(ICostoEnvioService costoEnvioService, IAuthorizationService authorizationService, INegocioService negocioService)
  {
    _costoEnvioService = costoEnvioService;
    _authorizationService = authorizationService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/costo-envio");
    Summary(s =>
    {
      s.Summary = "Crear Costo de Envío";
      s.Description = "Crea un nuevo costo de envío";
      s.ExampleRequest = new CreateCostoEnvioRequest
      {
        IdNegocio = Guid.NewGuid(),
        DistanciaMaxKm = 10,
        Costo = 100.50m
      };
    });
  }

  public override async Task HandleAsync(CreateCostoEnvioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Costo_Envio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      AddError(r => r.IdNegocio, "Negocio no encontrado");
    }

    var existingCostoEnvio = await _costoEnvioService.GetByNegocioAndDistanciaMaxKmAsync(req.IdNegocio, req.DistanciaMaxKm);
    if (existingCostoEnvio != null)
    {
      AddError(r => r.DistanciaMaxKm, "La distancia máxima en km ya está registrada para este negocio");
    }

    ThrowIfAnyErrors();

    var costoEnvio = new Domain.Entities.CostoEnvio
    {
      IdCostoEnvio = Guid.NewGuid(),
      IdNegocio = req.IdNegocio,
      DistanciaMaxKm = req.DistanciaMaxKm,
      Costo = req.Costo
    };

    await _costoEnvioService.AddAsync(costoEnvio);

    await SendOkAsync(costoEnvio, ct);
  }
}
