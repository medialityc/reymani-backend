using System;

namespace reymani_web_api.Domain.Entities;

public class PlantillaNotificacion
{
  public Guid IdPlantilla { get; set; } // PK, tipo Guid

  public required string Titulo { get; set; } // Título de la notificación
  public required string Contenido { get; set; } // Contenido o cuerpo de la notificación
  public Guid IdEstadoPedido { get; set; } // FK a Estado_Pedido
  public EstadoPedido? EstadoPedido { get; set; } // Propiedad de navegación a Estado_Pedido
  public DateTime FechaCreacion { get; set; } // Fecha de creación de la plantilla
  public string? Descripcion { get; set; } // Descripción opcional de la plantilla

  public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>(); // Relación de uno a muchos con Notificaciones
}

