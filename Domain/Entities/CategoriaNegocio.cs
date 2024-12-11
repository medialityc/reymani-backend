using System;

namespace reymani_web_api.Domain.Entities;

public class CategoriaNegocio
{
  public Guid IdCategoria { get; set; }  // PK, tipo Guid
  public required string Nombre { get; set; }     // Nombre de la categoría (Ej: "Restaurantes", "Ropa")
  public string? Descripcion { get; set; } // Descripción de la categoría

  // Relación 1:N con NegocioCategoria
  public ICollection<NegocioCategoria> NegociosCategorias { get; set; } = new List<NegocioCategoria>();
}

