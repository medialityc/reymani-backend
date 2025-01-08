using System;
using reymani_web_api.Application.DTOs;
using reymani_web_api.Application.Services;

namespace reymani_web_api.Api.Endpoints.CategoriaNegocio
{
  public class GetCategoriaNegocioByIdEndpoint : Endpoint<GetCategoriaNegocioByIdRequest, CategoriaNegocioDto>
  {
    private readonly ICategoriaNegocioService _categoriaNegocioService;
    private readonly IAuthorizationService _authorizationService;

    public GetCategoriaNegocioByIdEndpoint(ICategoriaNegocioService categoriaNegocioService, IAuthorizationService authorizationService)
    {
      _categoriaNegocioService = categoriaNegocioService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/categoria-negocio/{CategoriaNegocioId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener categoría de negocio por ID";
        s.Description = "Obtiene una categoría de negocio de la base de datos por su ID";
        s.ExampleRequest = new GetCategoriaNegocioByIdRequest
        {
          CategoriaNegocioId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new CategoriaNegocioDto
        {
          IdCategoria = Guid.NewGuid(),
          Nombre = "Restaurantes",
          Descripcion = "Categoría para negocios de comida"
        };
        s.Responses[404] = "Categoría de negocio no encontrada";
        s.Responses[200] = "Categoría de negocio encontrada";
      });
    }

    public override async Task HandleAsync(GetCategoriaNegocioByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Categoria_Negocio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var categoriaNegocio = await _categoriaNegocioService.GetByIdAsync(req.CategoriaNegocioId);
      if (categoriaNegocio == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var categoriaNegocioDto = new CategoriaNegocioDto
      {
        IdCategoria = categoriaNegocio.IdCategoria,
        Nombre = categoriaNegocio.Nombre,
        Descripcion = categoriaNegocio.Descripcion
      };

      await SendOkAsync(categoriaNegocioDto, ct);
    }
  }
}
