using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Producto
{
  public Guid IdProducto { get; set; }  // PK, tipo Guid
  public Guid IdNegocio { get; set; }   // FK, tipo Guid
  public required string Nombre { get; set; }    // Nombre del producto

  [StringLength(255)]
  public string? Descripcion { get; set; } // Descripción del producto
  public double Precio { get; set; }    // Precio del producto
  public double Stock { get; set; }         // Stock disponible
  public double StockCongelado { get; set; } // Stock congelado o reservado
  public bool Estado { get; set; } // Estado del producto (disponible, agotado)
  public string? URLImagen { get; set; } // URL de la imagen del producto
  public bool Activo { get; set; } // Indica si el producto está activo


  // Relación de muchos a uno con Negocio
  public Negocio? Negocio { get; set; }

  // Relación 1:N con ProductoCategoria
  public ICollection<ProductoCategoria> ProductosCategorias { get; set; } = new List<ProductoCategoria>();

}
