using System;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class ChangeClienteStatusRequest
{
  public Guid IdCliente { get; set; }
  public bool Activo { get; set; }
}
