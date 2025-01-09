using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CostoEnvio;

public sealed class GetAllCostoEnvioEndpoint : EndpointWithoutRequest<GetAllCostoEnvioResponse>
{
  private readonly ICostoEnvioService _costoEnvioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllCostoEnvioEndpoint(ICostoEnvioService costoEnvioService, IAuthorizationService authorizationService)
  {
    _costoEnvioService = costoEnvioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/costo-envio");
    Summary(s =>
    {
      s.Summary = "Obtener todos los costos de envío";
      s.Description = "Obtiene todos los costos de envío de la base de datos";
      s.ResponseExamples[200] = new GetAllCostoEnvioResponse
      {
        CostosEnvios = new List<CostoEnvioDto>
        {
          new CostoEnvioDto{
            IdCostoEnvio = Guid.NewGuid(),
            IdNegocio = Guid.NewGuid(),
            DistanciaMaxKm = 10,
            Costo = 100
          }
        }
      };
    });
  }

  public override async Task<GetAllCostoEnvioResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Costos_Envio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var costosEnvios = await _costoEnvioService.GetAllAsync();
    return new GetAllCostoEnvioResponse
    {
      CostosEnvios = costosEnvios.Select(ce => new CostoEnvioDto
      {
        IdCostoEnvio = ce.IdCostoEnvio,
        IdNegocio = ce.IdNegocio,
        DistanciaMaxKm = ce.DistanciaMaxKm,
        Costo = ce.Costo
      }).ToList()
    };
  }
}
