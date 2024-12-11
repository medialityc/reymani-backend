using System;

namespace reymani_web_api.Domain.Entities;

public class HistorialEstadoPedido
{
  public Guid IdHistorial { get; set; } // PK, tipo Guid
  public Guid IdPedido { get; set; }     // FK a Pedido
  public Pedido? Pedido { get; set; }     // Navegación a Pedido

  public Guid IdEstadoPedido { get; set; }     // FK a EstadoPedido
  public EstadoPedido? EstadoPedido { get; set; } // Navegación a EstadoPedido

  public DateTime FechaInicio { get; set; }    // Fecha en que comenzó el estado
  public DateTime? FechaFin { get; set; }      // Fecha en que terminó el estado (puede ser nula si es el estado actual)
}

