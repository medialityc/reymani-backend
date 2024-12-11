using System;

namespace reymani_web_api.Domain.Entities;

public class ProductoCategoria
{
  public Guid IdProductoCategoria { get; set; }                  // PK, tipo Guid
  public Guid IdProducto { get; set; }          // FK, tipo Guid
  public Guid IdCategoriaProducto { get; set; } // FK, tipo Guid

  // Relación de muchos a uno con Producto
  public Producto Producto { get; set; }

  // Relación de muchos a uno con CategoriaProducto
  public CategoriaProducto CategoriaProducto { get; set; }
}

