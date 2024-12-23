using System;

namespace reymani_web_api.Application.DTOs;

public class RolDto
{
  public Guid IdRol { get; set; } // PK, tipo Guid
  public string Nombre { get; set; } // Nombre del rol (ej. 'Administrador', 'Cliente', 'Mensajero', etc.)
  public string? Descripcion { get; set; } // Descripción opcional del rol

}
