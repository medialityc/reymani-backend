using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CategoriaNegocio;

public sealed class GetAllCategoriaNegocioEndpoint : EndpointWithoutRequest<GetAllCategoriaNegocioResponse>
{
  private readonly ICategoriaNegocioService _categoriaNegocioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllCategoriaNegocioEndpoint(ICategoriaNegocioService categoriaNegocioService, IAuthorizationService authorizationService)
  {
    _categoriaNegocioService = categoriaNegocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/categoria-negocio");
    Summary(s =>
    {
      s.Summary = "Obtener todas las categorías de negocio";
      s.Description = "Obtiene todas las categorías de negocio de la base de datos";
      s.ResponseExamples[200] = new GetAllCategoriaNegocioResponse
      {
        CategoriasNegocios = new List<CategoriaNegocioDto>
        {
          new CategoriaNegocioDto
          {
            IdCategoria = Guid.NewGuid(),
            Nombre = "Restaurantes",
            Descripcion = "Categoría para restaurantes"
          }
        }
      };
    });
  }

  public override async Task<GetAllCategoriaNegocioResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Categorias_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var categoriasNegocios = await _categoriaNegocioService.GetAllAsync();
    return new GetAllCategoriaNegocioResponse
    {
      CategoriasNegocios = categoriasNegocios.Select(c => new CategoriaNegocioDto
      {
        IdCategoria = c.IdCategoria,
        Nombre = c.Nombre,
        Descripcion = c.Descripcion
      }).ToList()
    };
  }
}
