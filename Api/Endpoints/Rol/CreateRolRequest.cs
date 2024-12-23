using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public class CreateRolRequest
{
  public required string Nombre { get; set; }
  public string? Descripcion { get; set; }
}
