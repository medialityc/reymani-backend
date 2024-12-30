using System;
using System.Collections.Generic;

namespace reymani_web_api.Api.Endpoints.Rol
{
  public class GetRolPermissionsByRolIdResponse
  {
    public List<Guid> PermisoIds { get; set; } = new List<Guid>();
  }
}
