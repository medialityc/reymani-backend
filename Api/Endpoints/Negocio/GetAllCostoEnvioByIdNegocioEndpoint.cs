using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using reymani_web_api.Application.DTOs;
using reymani_web_api.Application.Interfaces;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class GetAllCostoEnvioByIdNegocioEndpoint : Endpoint<GetAllCostoEnvioByIdNegocioRequest, GetAllCostoEnvioByIdNegocioResponse>
{
  private readonly ICostoEnvioService _costoEnvioService;
  private readonly INegocioService _negocioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllCostoEnvioByIdNegocioEndpoint(ICostoEnvioService costoEnvioService, INegocioService negocioService, IAuthorizationService authorizationService)
  {
    _costoEnvioService = costoEnvioService;
    _negocioService = negocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/negocio/{IdNegocio:guid}/costo-envio");
    Summary(s =>
    {
      s.Summary = "Obtener todos los costos de envío por IdNegocio";
      s.Description = "Obtiene todos los costos de envío asociados a un negocio específico";
      s.ResponseExamples[200] = new GetAllCostoEnvioByIdNegocioResponse
      {
        CostosEnvios = new List<CostoEnvioDto>
        {
          new CostoEnvioDto
          {
            IdCostoEnvio = Guid.NewGuid(),
            IdNegocio = Guid.NewGuid(),
            DistanciaMaxKm = 10,
            Costo = 100.00m
          }
        }
      };
    });
  }

  public override async Task<GetAllCostoEnvioByIdNegocioResponse> ExecuteAsync(GetAllCostoEnvioByIdNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Costos_Envio_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      AddError(r => r.IdNegocio, "Negocio no encontrado");
    }

    ThrowIfAnyErrors();

    var costosEnvios = await _costoEnvioService.GetAllAsync();
    var filteredCostosEnvios = costosEnvios.Where(ce => ce.IdNegocio == req.IdNegocio);

    return new GetAllCostoEnvioByIdNegocioResponse
    {
      CostosEnvios = filteredCostosEnvios.Select(ce => new CostoEnvioDto
      {
        IdCostoEnvio = ce.IdCostoEnvio,
        IdNegocio = ce.IdNegocio,
        DistanciaMaxKm = ce.DistanciaMaxKm,
        Costo = ce.Costo
      }).ToList()
    };
  }
}
