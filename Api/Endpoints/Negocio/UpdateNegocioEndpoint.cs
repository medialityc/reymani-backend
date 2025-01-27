using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class UpdateNegocioEndpoint : Endpoint<UpdateNegocioRequest>
{
  private readonly INegocioService _negocioService;

  private readonly IAuthorizationService _authorizationService;

  public UpdateNegocioEndpoint(INegocioService negocioService, IAuthorizationService authorizationService)
  {
    _negocioService = negocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/negocio/{IdNegocio:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Negocio";
      s.Description = "Actualiza un negocio en la base de datos";
      s.ExampleRequest = new UpdateNegocioRequest
      {
        IdNegocio = Guid.NewGuid(),
        Negocio = new NegocioDto
        {
          Nombre = "Nuevo Negocio",
          Descripcion = "Descripción del negocio",
          EntregaDomicilio = true
        }
      };
    });
  }

  public override async Task HandleAsync(UpdateNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.Negocio.IdNegocio != req.IdNegocio)
    {
      AddError(r => r.IdNegocio, "El ID del negocio no coincide con el ID de la URL");
    }

    ThrowIfAnyErrors();

    if (negocio != null)
    {
      negocio.Nombre = req.Negocio.Nombre;
      negocio.Descripcion = req.Negocio.Descripcion ?? negocio.Descripcion;
      negocio.EntregaDomicilio = req.Negocio.EntregaDomicilio;
      await _negocioService.UpdateAsync(negocio);
      await SendOkAsync(ct);
    }
  }
}
