using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Api.Endpoints.Direccion;

public class GetAllDireccionEndpoint : EndpointWithoutRequest<GetAllDireccionResponse>
{
  private readonly IDireccionService _direccionService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllDireccionEndpoint(IDireccionService direccionService, IAuthorizationService authorizationService)
  {
    _direccionService = direccionService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/direccion");
    Summary(s =>
    {
      s.Summary = "Obtener todas las direcciones";
      s.Description = "Obtiene todas las direcciones de la base de datos";
      s.ResponseExamples[200] = new GetAllDireccionResponse
      {
        Direcciones = new List<Domain.Entities.Direccion>
        {
          new Domain.Entities.Direccion
          {
            IdDireccion = Guid.NewGuid(),
            TipoEntidad = "Usuario",
            IdEntidad = Guid.NewGuid(),
            DireccionEntidad = "Calle Falsa 123",
            Municipio = "Ciudad Ejemplo",
            Provincia = "Provincia Ejemplo",
            Latitud = 0.0,
            Longitud = 0.0,
            Descripcion = "Dirección de ejemplo"
          }
        }
      };
    });
  }

  public override async Task<GetAllDireccionResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Direcciones"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var direcciones = await _direccionService.GetAllAsync();
    return new GetAllDireccionResponse
    {
      Direcciones = direcciones.ToList()
    };
  }
}
