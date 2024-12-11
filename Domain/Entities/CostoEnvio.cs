using System;

namespace reymani_web_api.Domain.Entities;

public class CostoEnvio
{
  public Guid IdCostoEnvio { get; set; }  // PK, tipo Guid
  public Guid IdNegocio { get; set; }    // FK, tipo Guid
  public int DistanciaMaxKm { get; set; }  // Distancia máxima en kilómetros
  public decimal Costo { get; set; }      // Costo de envío para la distancia especificada

  // Relación de muchos a uno con Negocio
  public Negocio? Negocio { get; set; }
}

