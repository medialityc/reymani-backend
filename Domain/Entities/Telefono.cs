using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Telefono
{
  public Guid IdTelefono { get; set; }

  [StringLength(50)]
  public required string TipoEntidad { get; set; }

  public Guid IdEntidad { get; set; }

  public required string NumeroTelefono { get; set; }

  [StringLength(100)]
  public string? Descripcion { get; set; }

}
