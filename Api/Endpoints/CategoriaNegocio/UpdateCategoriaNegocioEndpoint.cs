using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CategoriaNegocio;

public sealed class UpdateCategoriaNegocioEndpoint : Endpoint<UpdateCategoriaNegocioRequest>
{
  private readonly ICategoriaNegocioService _categoriaNegocioService;
  private readonly IAuthorizationService _authorizationService;

  public UpdateCategoriaNegocioEndpoint(ICategoriaNegocioService categoriaNegocioService, IAuthorizationService authorizationService)
  {
    _categoriaNegocioService = categoriaNegocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/categoria-negocio/{CategoriaNegocioId:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Categoría de Negocio";
      s.Description = "Actualiza una categoría de negocio";
      s.ExampleRequest = new UpdateCategoriaNegocioRequest
      {
        CategoriaNegocioId = Guid.NewGuid(),
        CategoriaNegocio = new CategoriaNegocioDto
        {
          Nombre = "Categoría de Prueba",
          Descripcion = "Descripción de Prueba"
        }
      };
    });
  }

  public override async Task HandleAsync(UpdateCategoriaNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Categoria_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var categoriaNegocio = await _categoriaNegocioService.GetByIdAsync(req.CategoriaNegocioId);
    if (categoriaNegocio == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.CategoriaNegocio.IdCategoria != req.CategoriaNegocioId)
    {
      AddError(r => r.CategoriaNegocioId, "El ID de la categoría de negocio no coincide con el ID de la URL");
    }

    ThrowIfAnyErrors();

    var updatedCategoriaNegocio = new Domain.Entities.CategoriaNegocio
    {
      IdCategoria = req.CategoriaNegocioId,
      Nombre = req.CategoriaNegocio.Nombre,
      Descripcion = req.CategoriaNegocio.Descripcion
    };

    await _categoriaNegocioService.UpdateAsync(updatedCategoriaNegocio);

    await SendOkAsync(updatedCategoriaNegocio, ct);
  }
}
