using System;

namespace reymani_web_api.Domain.Entities;

public class NegocioCliente
{
  public Guid IdNegocioCliente { get; set; } // PK, tipo Guid
  public Guid IdCliente { get; set; } // FK a Cliente
  public Cliente? Cliente { get; set; } // Propiedad de navegación a Cliente
  public Guid IdNegocio { get; set; } // FK a Negocio
  public Negocio? Negocio { get; set; } // Propiedad de navegación a Negocio

}
