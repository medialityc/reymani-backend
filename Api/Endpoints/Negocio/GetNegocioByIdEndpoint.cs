using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio
{
  public class GetNegocioByIdEndpoint : Endpoint<GetNegocioByIdRequest, NegocioDto>
  {
    private readonly INegocioService _negocioService;
    private readonly IAuthorizationService _authorizationService;

    public GetNegocioByIdEndpoint(INegocioService negocioService, IAuthorizationService authorizationService)
    {
      _negocioService = negocioService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/negocio/{NegocioId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener negocio por ID";
        s.Description = "Obtiene un negocio de la base de datos por su ID";
        s.ExampleRequest = new GetNegocioByIdRequest
        {
          NegocioId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new NegocioDto
        {
          IdNegocio = Guid.NewGuid(),
          Nombre = "Negocio Ejemplo",
          Descripcion = "Descripción del negocio",
          EntregaDomicilio = true,
          URLImagenPrincipal = "http://example.com/imagen.jpg",
          URLImagenLogo = "http://example.com/logo.jpg",
          URLImagenBanner = "http://example.com/banner.jpg"
        };
        s.Responses[404] = "Negocio no encontrado";
        s.Responses[200] = "Negocio encontrado";
      });
    }

    public override async Task HandleAsync(GetNegocioByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Negocio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var negocio = await _negocioService.GetByIdAsync(req.NegocioId);
      if (negocio == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var negocioDto = new NegocioDto
      {
        IdNegocio = negocio.IdNegocio,
        Nombre = negocio.Nombre,
        Descripcion = negocio.Descripcion,
        EntregaDomicilio = negocio.EntregaDomicilio,
        URLImagenPrincipal = negocio.URLImagenPrincipal,
        URLImagenLogo = negocio.URLImagenLogo,
        URLImagenBanner = negocio.URLImagenBanner
      };

      await SendOkAsync(negocioDto, ct);
    }
  }
}
