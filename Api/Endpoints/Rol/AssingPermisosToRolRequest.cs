using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public class AssingPermisosToRolRequest
{
  public Guid RolId { get; set; }
  public List<Guid> PermisoIds { get; set; } = new List<Guid>();
}
