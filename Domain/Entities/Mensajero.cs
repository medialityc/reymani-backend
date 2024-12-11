using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Mensajero
{
  public Guid IdMensajero { get; set; }  // PK

  [StringLength(50)]
  public required string Vehiculo { get; set; }  // Vehículo del mensajero (motocicleta, bicicleta, etc.)
  public required Boolean Estado { get; set; }  // Estado del mensajero (disponible, ocupado,)
  public required Boolean Activo { get; set; } //Indica si esta activo



  // Relación 1:N con Pedido (un mensajero puede estar asociado a varios pedidos)
  public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

  // Relación 1:N con MensajeroNegocio (un mensajero puede estar asociado a varios negocios)
  public ICollection<MensajeroNegocio> MensajerosNegocios { get; set; } = new List<MensajeroNegocio>();
}
