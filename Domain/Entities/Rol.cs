using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Rol
{
  public Guid IdRol { get; set; } // PK, tipo Guid

  [Required]
  [StringLength(50)]
  public required string Nombre { get; set; } // Nombre del rol (ej. 'Administrador', 'Usuario', 'Mensajero', etc.)

  [StringLength(100)]
  public string? Descripcion { get; set; } // Descripción opcional del rol

  public ICollection<UsuarioRol> Usuarios { get; set; } = new List<UsuarioRol>(); // Relación de uno a muchos con Usuario-Rol
  public ICollection<RolPermiso> Permisos { get; set; } = new List<RolPermiso>(); // Relación de uno a muchos con Rol-Permiso
}

