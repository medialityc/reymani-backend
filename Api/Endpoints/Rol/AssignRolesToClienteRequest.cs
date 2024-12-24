using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public class AssignRolesToClienteRequest
{
  public Guid ClienteId { get; set; }
  public List<Guid> RoleIds { get; set; } = new List<Guid>();
}
