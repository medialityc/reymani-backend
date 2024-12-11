using System;

namespace reymani_web_api.Domain.Entities;

public class CategoriaProducto
{
  public Guid IdCategoriaProducto { get; set; } // PK, tipo Guid
  public required string Nombre { get; set; }           // Nombre de la categoría (Ej: "Electrónica", "Comida")
  public string? Descripcion { get; set; }      // Descripción de la categoría

  // Relación 1:N con ProductoCategoria
  public ICollection<ProductoCategoria> ProductosCategorias { get; set; } = new List<ProductoCategoria>();
}

