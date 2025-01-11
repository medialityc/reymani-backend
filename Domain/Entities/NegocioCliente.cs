using System;
using System.Text.Json.Serialization;

namespace reymani_web_api.Domain.Entities;

public class NegocioCliente
{
  public Guid IdNegocioCliente { get; set; } // PK, tipo Guid
  public Guid IdCliente { get; set; } // FK a Cliente
  [JsonIgnore]
  public Cliente? Cliente { get; set; } // Propiedad de navegación a Cliente
  public Guid IdNegocio { get; set; } // FK a Negocio
  [JsonIgnore]
  public Negocio? Negocio { get; set; } // Propiedad de navegación a Negocio

}
