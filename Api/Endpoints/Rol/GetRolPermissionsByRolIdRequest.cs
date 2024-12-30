using System;

namespace reymani_web_api.Api.Endpoints.Rol
{
  public class GetRolPermissionsByRolIdRequest
  {
    public required Guid RolId { get; set; }
  }
}
