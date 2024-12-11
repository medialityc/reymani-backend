using System;

namespace reymani_web_api.Domain.Entities;

public class EstadoPedido
{
  public Guid IdEstado { get; set; }                  // PK, tipo Guid
  public required string Nombre { get; set; }                   // Nombre del estado, por ejemplo "Pendiente", "Enviado"
  public string? Descripcion { get; set; }              // Descripción opcional del estado
  public int TiempoDuracionEstimado { get; set; }      // Duración estimada en minutos
  public Guid IdPlantillaNotificacion { get; set; }   // FK opcional

  // Relación 1:N con Pedido
  public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

  public PlantillaNotificacion? PlantillaNotificacion { get; set; } // Navegación a PlantillaNotificacion

  // Relación 1:N con HistorialEstadoPedido
  public ICollection<HistorialEstadoPedido> HistorialEstadosPedido { get; set; } = new List<HistorialEstadoPedido>();

}

