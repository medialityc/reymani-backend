using System;

namespace reymani_web_api.Api.Endpoints.NegocioCliente
{
  public class CreateNegocioClienteRequest
  {
    public Guid IdCliente { get; set; }
    public Guid IdNegocio { get; set; }
  }
}
