using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Permiso;

public class GetAllPermisoResponse
{
  public List<PermisoDto>? Permisos { get; set; }
}
