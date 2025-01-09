using System;

namespace reymani_web_api.Application.DTOs;

public class CostoEnvioDto
{
  public Guid IdCostoEnvio { get; set; }
  public Guid IdNegocio { get; set; }
  public int DistanciaMaxKm { get; set; }
  public decimal Costo { get; set; }
}
