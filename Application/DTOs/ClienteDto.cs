using System;

namespace reymani_web_api.Application.DTOs;

public class ClienteDto
{
  public Guid Id { get; set; }
  public string NumeroCarnet { get; set; }
  public string Nombre { get; set; }
  public string Apellidos { get; set; }
  public string Username { get; set; }
}
