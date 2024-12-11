using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Direccion
{
  public Guid IdDireccion { get; set; } // PK, tipo Guid

  [StringLength(50)]
  public required string TipoEntidad { get; set; } // Tipo de entidad a la que pertenece la dirección (Negocio, Cliente, etc.)
  public Guid IdEntidad { get; set; } // FK, tipo Guid

  [StringLength(255)]
  public required string DireccionEntidad { get; set; } // Dirección del negocio

  [StringLength(100)]
  public string? Municipio { get; set; } // Ciudad de la dirección

  [StringLength(100)]
  public string? Provincia { get; set; } // Departamento de la dirección

  public double Latitud { get; set; } // Latitud de la dirección
  public double Longitud { get; set; } // Longitud de la dirección

  [StringLength(255)]
  public string? Descripcion { get; set; } // Notas adicionales sobre la dirección

}
