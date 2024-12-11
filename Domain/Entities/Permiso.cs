using System;

namespace reymani_web_api.Domain.Entities;

public class Permiso
{
  public Guid IdPermiso { get; set; } // PK, tipo Guid
  public required string Nombre { get; set; } // Nombre del permiso (ej. 'Crear Pedido', 'Ver Reportes', etc.)
  public string? Descripcion { get; set; } // Descripción opcional del permiso

  public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>(); // Relación de uno a muchos con Rol-Permiso
}

