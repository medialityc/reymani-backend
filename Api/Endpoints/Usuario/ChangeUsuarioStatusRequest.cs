using System;

namespace reymani_web_api.Api.Endpoints.Usuario;

public class ChangeUsuarioStatusRequest
{
  public Guid IdUsuario { get; set; }
  public bool Activo { get; set; }
}
