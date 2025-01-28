using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Usuario
{
  [Key]
  public Guid IdUsuario { get; set; }  // PK

  [StringLength(11)]
  public required string NumeroCarnet { get; set; }

  [StringLength(100)]
  public required string Nombre { get; set; }

  [StringLength(255)]
  public required string Apellidos { get; set; }

  [StringLength(50)]
  public required string Username { get; set; }

  [StringLength(256)]
  public required string PasswordHash { get; set; }

  public DateTime FechaRegistro { get; set; }

  public bool Activo { get; set; }

  // Relación 1:N con Pedido
  public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

  // Relación 1:N con Notificacion
  public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

  // Relación 1:N con Usuario-Rol
  public ICollection<UsuarioRol> Roles { get; set; } = new List<UsuarioRol>();

  // Relación 1:N con Negocio_Usuario
  public ICollection<NegocioUsuario> Negocios { get; set; } = new List<NegocioUsuario>();
}