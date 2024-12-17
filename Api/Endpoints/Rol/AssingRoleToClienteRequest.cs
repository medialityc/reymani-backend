using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public class AssingRoleToClienteRequest
{
  public Guid ClienteId { get; set; }
  public Guid RoleId { get; set; }
}
