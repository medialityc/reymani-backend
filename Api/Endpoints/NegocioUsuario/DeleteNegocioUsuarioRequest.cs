using System;

namespace reymani_web_api.Api.Endpoints.NegocioUsuario;

public class DeleteNegocioUsuarioRequest
{
  public Guid UsuarioId { get; set; }
  public Guid NegocioId { get; set; }
}
