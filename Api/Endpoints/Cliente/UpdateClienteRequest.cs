using System;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class UpdateClienteRequest
{
  public Guid IdCliente { get; set; }
  public required string NumeroCarnet { get; set; }
  public required string Nombre { get; set; }
  public required string Apellidos { get; set; }
  public required string Username { get; set; }
  public required string Password { get; set; }
}
