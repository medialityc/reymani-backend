using System;
using System.ComponentModel.DataAnnotations;

namespace reymani_web_api.Domain.Entities;

public class Negocio
{
    public Guid IdNegocio { get; set; }  // PK

    [Required]
    [StringLength(100)]
    public required string Nombre { get; set; }  // Nombre del negocio

    [StringLength(500)]
    public required string Descripcion { get; set; } // Descripción del negocio

    public bool EntregaDomicilio { get; set; } // Indica si el negocio ofrece entrega a domicilio

    public string? URLImagenPrincipal { get; set; } // URL de la imagen principal del negocio

    public string? URLImagenLogo { get; set; } // URL del logo del negocio

    public string? URLImagenBanner { get; set; } // URL de la imagen de portada del negocio

    // Relación 1:N con Producto (un negocio puede tener múltiples productos)
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();

    // Relación 1:N con HorarioNegocio (un negocio tiene múltiples horarios de apertura)
    public ICollection<HorarioNegocio> Horarios { get; set; } = new List<HorarioNegocio>();

    // Relación 1:N con CostoEnvio (un negocio puede tener varios costos de envío)
    public ICollection<CostoEnvio> CostosEnvio { get; set; } = new List<CostoEnvio>();

    public ICollection<NegocioCategoria> NegocioCategorias { get; set; } = new List<NegocioCategoria>();

    // Relación 1:N con MensajeroNegocio (un negocio puede estar asociado a varios mensajeros)
    public ICollection<MensajeroNegocio> MensajerosNegocio { get; set; } = new List<MensajeroNegocio>();

    // Relación 1:N con NegocioCliente (un negocio puede tener varios clientes)
    public ICollection<NegocioCliente> Clientes { get; set; } = new List<NegocioCliente>();
}

