using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Permiso;

public sealed class GetAllPermisoEndpoint : EndpointWithoutRequest<GetAllPermisoResponse>
{
  private readonly IPermisoService _permisoService;

  public GetAllPermisoEndpoint(IPermisoService permisoService)
  {
    _permisoService = permisoService;
  }

  public override void Configure()
  {
    Get("/permiso");
    Summary(s =>
    {
      s.Summary = "Obtener todos los permisos";
      s.Description = "Obtiene todos los permisos de la base de datos";
      s.ResponseExamples[200] = new GetAllPermisoResponse
      {
        Permisos = new List<PermisoDto>
        {
          new PermisoDto
          {
            Id = Guid.NewGuid(),
            Codigo = "PERMISO",
            Descripcion = "Permiso de acceso"
          }
        }
      };
    });
  }

  public override async Task<GetAllPermisoResponse> ExecuteAsync(CancellationToken ct)
  {
    var permisos = await _permisoService.GetAllAsync();
    return new GetAllPermisoResponse
    {
      Permisos = permisos.Select(p => new PermisoDto
      {
        Id = p.IdPermiso,
        Codigo = p.Codigo,
        Descripcion = p.Descripcion ?? string.Empty
      }).ToList()
    };

  }
}