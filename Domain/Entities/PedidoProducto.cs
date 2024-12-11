using System;

namespace reymani_web_api.Domain.Entities;

public class PedidoProducto
{
  public Guid IdPedidoProducto { get; set; } // PK, tipo Guid

  public Guid IdPedido { get; set; } // FK a Pedido
  public Pedido Pedido { get; set; } // Navegación a Pedido

  public Guid IdProducto { get; set; } // FK a Producto
  public Producto Producto { get; set; } // Navegación a Producto

  public int Cantidad { get; set; } // Cantidad de productos en el pedido
  public string Notas { get; set; } // Notas adicionales sobre el producto
  public double Subtotal { get; set; } // Subtotal por el producto (Cantidad * Precio unitario)
}

