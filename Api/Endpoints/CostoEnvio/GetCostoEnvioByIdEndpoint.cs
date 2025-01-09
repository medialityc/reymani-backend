using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CostoEnvio
{
  public class GetCostoEnvioByIdEndpoint : Endpoint<GetCostoEnvioByIdRequest, CostoEnvioDto>
  {
    private readonly ICostoEnvioService _costoEnvioService;
    private readonly IAuthorizationService _authorizationService;

    public GetCostoEnvioByIdEndpoint(ICostoEnvioService costoEnvioService, IAuthorizationService authorizationService)
    {
      _costoEnvioService = costoEnvioService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/costo-envio/{CostoEnvioId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener costo de envío por ID";
        s.Description = "Obtiene un costo de envío de la base de datos por su ID";
        s.ExampleRequest = new GetCostoEnvioByIdRequest
        {
          CostoEnvioId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new CostoEnvioDto
        {
          IdCostoEnvio = Guid.NewGuid(),
          IdNegocio = Guid.NewGuid(),
          DistanciaMaxKm = 10,
          Costo = 100.00m
        };
        s.Responses[404] = "Costo de envío no encontrado";
        s.Responses[200] = "Costo de envío encontrado";
      });
    }

    public override async Task HandleAsync(GetCostoEnvioByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Costo_Envio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var costoEnvio = await _costoEnvioService.GetByIdAsync(req.CostoEnvioId);
      if (costoEnvio == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var costoEnvioDto = new CostoEnvioDto
      {
        IdCostoEnvio = costoEnvio.IdCostoEnvio,
        IdNegocio = costoEnvio.IdNegocio,
        DistanciaMaxKm = costoEnvio.DistanciaMaxKm,
        Costo = costoEnvio.Costo
      };

      await SendOkAsync(costoEnvioDto, ct);
    }
  }
}
