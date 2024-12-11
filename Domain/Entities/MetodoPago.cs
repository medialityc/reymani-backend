using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class MetodoPago
{
  public Guid IdMetodoPago { get; set; } // PK, tipo Guid

  public required string TipoEntidad { get; set; }

  public Guid IdEntidad { get; set; } // FK, tipo Guid

  public required string Proveedor { get; set; } // Nombre del proveedor de pago (ej. Efectivo, Tropipay, etc.)

  public DateTime FechaExpiracion { get; set; } // Fecha de expiración del método de pago

  public bool Activo { get; set; }
  public DateTime FechaRegistro { get; set; } // Fecha en la que se registró el método de pago


  [StringLength(256)]
  public string? Dato1 { get; set; } // Datos adicionales del método de pago

  [StringLength(256)]
  public string? Dato2 { get; set; } // Datos adicionales del método de pago

  [StringLength(256)]
  public string? Dato3 { get; set; } // Datos adicionales del método de pago

}
