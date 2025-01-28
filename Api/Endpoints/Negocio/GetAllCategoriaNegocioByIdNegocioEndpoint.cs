using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class GetAllCategoriaNegocioByIdNegocioEndpoint : Endpoint<GetAllCategoriaNegocioByIdNegocioRequest, GetAllCategoriaNegocioByIdNegocioResponse>
{
  private readonly ICategoriaNegocioService _categoriaNegocioService;
  private readonly INegocioService _negocioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllCategoriaNegocioByIdNegocioEndpoint(ICategoriaNegocioService categoriaNegocioService, INegocioService negocioService, IAuthorizationService authorizationService)
  {
    _categoriaNegocioService = categoriaNegocioService;
    _negocioService = negocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/negocio/{IdNegocio:guid}/categoria-negocio");
    Summary(s =>
    {
      s.Summary = "Obtener todas las categorías de negocio por IdNegocio";
      s.Description = "Obtiene todas las categorías de negocio asociadas a un negocio específico";
      s.ResponseExamples[200] = new GetAllCategoriaNegocioByIdNegocioResponse
      {
        CategoriasNegocios = new List<CategoriaNegocioDto>
        {
          new CategoriaNegocioDto
          {
            IdCategoria = Guid.NewGuid(),
            Descripcion = "Categoría Ejemplo",
            Nombre = "Categoría Ejemplo"
          }
        }
      };
    });
  }

  public override async Task<GetAllCategoriaNegocioByIdNegocioResponse> ExecuteAsync(GetAllCategoriaNegocioByIdNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Categorias_Negocio_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      AddError(r => r.IdNegocio, "Negocio no encontrado");
    }

    ThrowIfAnyErrors();

    var filteredCategoriasNegocios = await _categoriaNegocioService.GetAllByIdNegocioAsync(req.IdNegocio);

    return new GetAllCategoriaNegocioByIdNegocioResponse
    {
      CategoriasNegocios = filteredCategoriasNegocios.Select(cn => new CategoriaNegocioDto
      {
        IdCategoria = cn.IdCategoria,
        Descripcion = cn.Descripcion,
        Nombre = cn.Nombre
      }).ToList()
    };
  }
}
