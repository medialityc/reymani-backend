using System;

namespace reymani_web_api.Domain.Entities;

public class MensajeroNegocio
{
  public Guid IdMensajeroNegocio { get; set; }
  public Guid IdNegocio { get; set; }
  public Guid IdMensajero { get; set; }
  public DateTime FechaAsociacion { get; set; }

  public Negocio Negocio { get; set; }
  public Mensajero Mensajero { get; set; }
}
