using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Imagen
{
  public Guid IdImagen { get; set; }           // PK, tipo Guid

  [StringLength(50)]
  public required string TipoEntidad { get; set; } // Tipo de entidad a la que pertenece la imagen (Producto, Negocio, etc.)

  public Guid IdEntidad { get; set; } // FK, tipo Guid

  public required string Url { get; set; }              // URL de la imagen

  [StringLength(255)]
  public string? Descripcion { get; set; }      // Descripción de la imagen

}

