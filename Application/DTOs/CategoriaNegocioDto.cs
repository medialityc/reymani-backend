using System;

namespace reymani_web_api.Application.DTOs;

public class CategoriaNegocioDto
{
  public Guid IdCategoria { get; set; }  // PK, tipo Guid
  public required string Nombre { get; set; }     // Nombre de la categoría (Ej: "Restaurantes", "Ropa")
  public string? Descripcion { get; set; } // Descripción de la categoría

}
