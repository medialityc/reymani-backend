using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Pedido
{
  public Guid IdPedido { get; set; }           // PK, tipo Guid
  public Guid IdUsuario { get; set; }          // FK, tipo Guid
  public Guid IdMensajero { get; set; }        // FK, tipo Guid
  public Guid IdNegocio { get; set; }          // FK, tipo Guid
  public Guid DireccionEntrega { get; set; }   // FK, tipo Guid
  public Guid IdMetodoPago { get; set; }       // FK, tipo Guid
  public bool ConfirmacionPago { get; set; }   // Confirmación de pago
  public Guid IdEstado { get; set; }           // FK, tipo Guid
  public double Total { get; set; }           // Total del pedido
  public double CostoEnvio { get; set; }      // Costo de envío
  public double SubtotalProductos { get; set; } // Costo de elaboración
  public DateTime FechaCreacion { get; set; }  // Fecha de creación
  public DateTime FechaFinalizacion { get; set; } // Fecha de finalización (nullable)
  public int Valoracion { get; set; }     // Valoración del pedido (opcional)
  public string? Descripcion { get; set; }      // Descripción adicional del pedido
  public bool cerrado { get; set; } // Indica si el pedido ha sido cerrado
  public double Descuento { get; set; }        // Descuento del pedido

  [StringLength(255)]
  public string? DescripcionDescuento { get; set; } // Descripción del descuento

  // Relación muchos a uno con Usuario
  public Usuario? Usuario { get; set; }

  // Relación muchos a uno con Mensajero
  public Mensajero? Mensajero { get; set; }

  // Relación muchos a uno con Negocio
  public Negocio? Negocio { get; set; }

  // Relación muchos a uno con Direccion
  public Direccion? Direccion { get; set; }

  // Relación muchos a uno con MetodoPago
  public MetodoPago? MetodoPago { get; set; }

  // Relación muchos a uno con EstadoPedido
  public EstadoPedido? EstadoPedido { get; set; }

  // Relación 1:N con PedidoProducto
  public ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();

  // Relación 1:N con Notificacion
  public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

  // Relación de 1 a muchos con HistorialEstadoPedido
  public ICollection<HistorialEstadoPedido>? HistorialEstadosPedido { get; set; }
}

