using System;

namespace reymani_web_api.Domain.Entities;

public class RolPermiso
{
  public Guid IdRolPermiso { get; set; } // PK, tipo Guid
  public Guid IdRol { get; set; } // FK a Rol
  public Rol? Rol { get; set; } // Navegación a Rol
  public Guid IdPermiso { get; set; } // FK a Permiso
  public Permiso? Permiso { get; set; } // Navegación a Permiso
}

