using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol;

public class UpdateRolRequest
{
  public Guid RolId { get; set; }

  public required RolDto Rol { get; set; }
}

