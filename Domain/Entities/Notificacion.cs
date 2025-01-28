using System;

namespace reymani_web_api.Domain.Entities;

public class Notificacion
{
  public Guid IdNotificacion { get; set; } // PK, tipo Guid

  public Guid IdUsuario { get; set; } // FK a Usuario
  public Usuario? Usuario { get; set; } // Navegación a Usuario

  public Guid IdPedido { get; set; } // FK a Pedido
  public Pedido? Pedido { get; set; } // Navegación a Pedido

  public Guid IdNegocio { get; set; } // FK a Negocio
  public Negocio? Negocio { get; set; } // Navegación a Negocio

  public Guid IdPlantilla { get; set; } // FK a Plantilla_Notificacion
  public PlantillaNotificacion? PlantillaNotificacion { get; set; } // Navegación a Plantilla_Notificacion
  public DateTime FechaEnvio { get; set; } // Fecha en que la notificación fue enviada (puede ser nula si aún no se ha enviado)
}

