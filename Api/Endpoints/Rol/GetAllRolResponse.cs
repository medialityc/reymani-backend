using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol;

public class GetAllRolResponse
{
  public List<RolDto> Roles { get; set; } = new List<RolDto>();

}
