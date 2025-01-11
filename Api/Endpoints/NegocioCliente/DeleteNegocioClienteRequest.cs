using System;

namespace reymani_web_api.Api.Endpoints.NegocioCliente;

public class DeleteNegocioClienteRequest
{
  public Guid ClienteId { get; set; }
  public Guid NegocioId { get; set; }
}
