namespace reymani_web_api.Domain.Entities;

public class NegocioCategoria
{
  public Guid IdNegocioCategoria { get; set; }         // PK, tipo Guid
  public Guid IdNegocio { get; set; }  // FK, tipo Guid
  public Guid IdCategoria { get; set; } // FK, tipo Guid

  // Relación de muchos a uno con Negocio
  public required Negocio Negocio { get; set; }

  // Relación de muchos a uno con CategoriaNegocio
  public required CategoriaNegocio Categoria { get; set; }
}

