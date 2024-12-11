using System;

namespace reymani_web_api.Domain.Entities;

public class ClienteRol
{
  public Guid IdClienteRol { get; set; } // PK, tipo Guid
  public Guid IdCliente { get; set; } // FK a Cliente
  public Cliente? Cliente { get; set; } // Navegación a Cliente
  public Guid IdRol { get; set; } // FK a Rol
  public Rol? Rol { get; set; } // Navegación a Rol
}

