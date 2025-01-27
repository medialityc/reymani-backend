using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class GetAllNegocioEndpoint : EndpointWithoutRequest<GetAllNegocioResponse>
{
  private readonly INegocioService _negocioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllNegocioEndpoint(INegocioService negocioService, IAuthorizationService authorizationService)
  {
    _negocioService = negocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/negocio");
    Summary(s =>
    {
      s.Summary = "Obtener todos los negocios";
      s.Description = "Obtiene todos los negocios de la base de datos";
      s.ResponseExamples[200] = new GetAllNegocioResponse
      {
        Negocios = new List<NegocioDto>
        {
          new NegocioDto
          {
            IdNegocio = Guid.NewGuid(),
            Nombre = "Negocio Ejemplo",
            Descripcion = "Descripción del negocio",
            EntregaDomicilio = true
          }
        }
      };
    });
  }

  public override async Task<GetAllNegocioResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Negocios"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocios = await _negocioService.GetAllAsync();
    return new GetAllNegocioResponse
    {
      Negocios = negocios.Select(n => new NegocioDto
      {
        IdNegocio = n.IdNegocio,
        Nombre = n.Nombre,
        Descripcion = n.Descripcion,
        EntregaDomicilio = n.EntregaDomicilio
      }).ToList()
    };
  }
}
