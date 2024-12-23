using System;

namespace reymani_web_api.Application.DTOs;

public class PermisoDto
{
  public Guid Id { get; set; }
  public required string Codigo { get; set; }
  public string? Descripcion { get; set; }
}
