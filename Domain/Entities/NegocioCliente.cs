using System;
using System.Text.Json.Serialization;

namespace reymani_web_api.Domain.Entities;

public class NegocioUsuario
{
  public Guid IdNegocioUsuario { get; set; } // PK, tipo Guid
  public Guid IdUsuario { get; set; } // FK a Usuario
  [JsonIgnore]
  public Usuario? Usuario { get; set; } // Propiedad de navegación a Usuario
  public Guid IdNegocio { get; set; } // FK a Negocio
  [JsonIgnore]
  public Negocio? Negocio { get; set; } // Propiedad de navegación a Negocio

}
