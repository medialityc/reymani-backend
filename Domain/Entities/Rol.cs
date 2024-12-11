using System;

namespace reymani_web_api.Domain.Entities;

public class Rol
{
  public Guid IdRol { get; set; } // PK, tipo Guid
  public required string Nombre { get; set; } // Nombre del rol (ej. 'Administrador', 'Cliente', 'Mensajero', etc.)
  public string? Descripcion { get; set; } // Descripción opcional del rol

  public ICollection<ClienteRol> Clientes { get; set; } = new List<ClienteRol>(); // Relación de uno a muchos con Cliente-Rol
  public ICollection<RolPermiso> Permisos { get; set; } = new List<RolPermiso>(); // Relación de uno a muchos con Rol-Permiso
}

