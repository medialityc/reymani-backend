using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Permiso
{
  public Guid IdPermiso { get; set; } // PK, tipo Guid

  [Required]
  [StringLength(50)]
  public required string Codigo { get; set; } // Codigo del permiso

  [StringLength(100)]
  public string? Descripcion { get; set; } // Descripción opcional del permiso

  public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>(); // Relación de uno a muchos con Rol-Permiso
}

