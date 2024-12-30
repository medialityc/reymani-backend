using System;
using System.Collections.Generic;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol
{
  public class GetRolPermissionsByRolIdResponse
  {
    public List<PermisoDto> Permisos { get; set; } = new List<PermisoDto>();
  }
}
