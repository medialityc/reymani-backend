using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public class AssignRolesToUsuarioRequest
{
  public Guid UsuarioId { get; set; }
  public List<Guid> RoleIds { get; set; } = new List<Guid>();
}
