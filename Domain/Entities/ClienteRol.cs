using System;

namespace reymani_web_api.Domain.Entities;

public class UsuarioRol
{
  public Guid IdUsuarioRol { get; set; } // PK, tipo Guid
  public Guid IdUsuario { get; set; } // FK a Usuario
  public Usuario? Usuario { get; set; } // Navegación a Usuario
  public Guid IdRol { get; set; } // FK a Rol
  public Rol? Rol { get; set; } // Navegación a Rol
}

