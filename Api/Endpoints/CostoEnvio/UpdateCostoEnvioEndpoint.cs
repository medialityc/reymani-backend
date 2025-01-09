using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CostoEnvio;

public sealed class UpdateCostoEnvioEndpoint : Endpoint<UpdateCostoEnvioRequest>
{
  private readonly ICostoEnvioService _costoEnvioService;
  private readonly IAuthorizationService _authorizationService;
  private readonly INegocioService _negocioService;

  public UpdateCostoEnvioEndpoint(ICostoEnvioService costoEnvioService, IAuthorizationService authorizationService, INegocioService negocioService)
  {
    _costoEnvioService = costoEnvioService;
    _authorizationService = authorizationService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/costo-envio/{CostoEnvioId:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Costo de Envío";
      s.Description = "Actualiza un costo de envío";
      s.ExampleRequest = new UpdateCostoEnvioRequest
      {
        CostoEnvioId = Guid.NewGuid(),
        CostoEnvio = new CostoEnvioDto
        {
          IdCostoEnvio = Guid.NewGuid(),
          IdNegocio = Guid.NewGuid(),
          DistanciaMaxKm = 10,
          Costo = 100.0m
        }
      };
    });
  }

  public override async Task HandleAsync(UpdateCostoEnvioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Costo_Envio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var costoEnvio = await _costoEnvioService.GetByIdAsync(req.CostoEnvioId);
    if (costoEnvio == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.CostoEnvio.IdCostoEnvio != req.CostoEnvioId)
    {
      AddError(r => r.CostoEnvioId, "El ID del costo de envío no coincide con el ID de la URL");
    }

    var negocio = await _negocioService.GetByIdAsync(req.CostoEnvio.IdNegocio);
    if (negocio == null)
    {
      AddError(r => r.CostoEnvio.IdNegocio, "Negocio no encontrado");
    }

    ThrowIfAnyErrors();

    var updatedCostoEnvio = new Domain.Entities.CostoEnvio
    {
      IdCostoEnvio = req.CostoEnvioId,
      IdNegocio = req.CostoEnvio.IdNegocio,
      DistanciaMaxKm = req.CostoEnvio.DistanciaMaxKm,
      Costo = req.CostoEnvio.Costo
    };

    await _costoEnvioService.UpdateAsync(updatedCostoEnvio);

    await SendOkAsync(updatedCostoEnvio, ct);
  }
}
