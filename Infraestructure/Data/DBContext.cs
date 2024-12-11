using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace reymani_web_api.Infraestructure.Data;

public class DBContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Mensajero> Mensajeros { get; set; }
    public DbSet<MensajeroNegocio> MensajeroNegocios { get; set; }
    public DbSet<Telefono> Telefonos { get; set; }
    public DbSet<Negocio> Negocios { get; set; }
    public DbSet<HorarioNegocio> HorariosNegocios { get; set; }
    public DbSet<CostoEnvio> CostosEnvios { get; set; }
    public DbSet<CategoriaNegocio> CategoriasNegocios { get; set; }
    public DbSet<NegocioCategoria> NegocioCategorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<CategoriaProducto> CategoriasProducto { get; set; }
    public DbSet<ProductoCategoria> ProductosCategorias { get; set; }
    public DbSet<Imagen> Imagenes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<EstadoPedido> EstadosPedido { get; set; }
    public DbSet<HistorialEstadoPedido> HistorialEstadosPedido { get; set; }
    public DbSet<PedidoProducto> PedidosProductos { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<PlantillaNotificacion> PlantillasNotificacion { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Permiso> Permisos { get; set; }
    public DbSet<ClienteRol> ClientesRoles { get; set; }
    public DbSet<RolPermiso> RolesPermisos { get; set; }
    public DbSet<MetodoPago> MetodosPago { get; set; }
    public DbSet<NegocioCliente> NegociosClientes { get; set; }
    public DbSet<Direccion> Direcciones { get; set; }

    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de relaciones y restricciones para cada entidad

        // Relación Cliente - Pedido
        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Pedidos)
            .WithOne(p => p.Cliente)
            .HasForeignKey(p => p.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Cliente - ClienteRol
        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Roles)
            .WithOne(cr => cr.Cliente)
            .HasForeignKey(cr => cr.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación ClienteRol - Rol
        modelBuilder.Entity<ClienteRol>()
            .HasOne(cr => cr.Rol)
            .WithMany(r => r.Clientes)
            .HasForeignKey(cr => cr.IdRol)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Rol - Permiso a través de RolPermiso
        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Rol)
            .WithMany(r => r.Permisos)
            .HasForeignKey(rp => rp.IdRol)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Permiso)
            .WithMany(p => p.RolPermisos)
            .HasForeignKey(rp => rp.IdPermiso)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Negocio - CostoEnvio
        modelBuilder.Entity<Negocio>()
            .HasMany(n => n.CostosEnvio)
            .WithOne(ce => ce.Negocio)
            .HasForeignKey(ce => ce.IdNegocio)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Negocio - HorarioNegocio
        modelBuilder.Entity<Negocio>()
            .HasMany(n => n.Horarios)
            .WithOne(hn => hn.Negocio)
            .HasForeignKey(hn => hn.IdNegocio)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Negocio - Producto
        modelBuilder.Entity<Negocio>()
            .HasMany(n => n.Productos)
            .WithOne(p => p.Negocio)
            .HasForeignKey(p => p.IdNegocio)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Pedido - PedidoProducto
        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.PedidoProductos)
            .WithOne(pp => pp.Pedido)
            .HasForeignKey(pp => pp.IdPedido)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Pedido - EstadoPedido
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.EstadoPedido)
            .WithMany()
            .HasForeignKey(p => p.IdEstado)
            .OnDelete(DeleteBehavior.SetNull);

        // Relación Pedido - HistorialEstadoPedido
        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.HistorialEstadosPedido)
            .WithOne(hep => hep.Pedido)
            .HasForeignKey(hep => hep.IdPedido)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Producto - ProductoCategoria
        modelBuilder.Entity<ProductoCategoria>()
            .HasOne(pc => pc.Producto)
            .WithMany(p => p.ProductosCategorias)
            .HasForeignKey(pc => pc.IdProducto)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductoCategoria>()
            .HasOne(pc => pc.CategoriaProducto)
            .WithMany(cp => cp.ProductosCategorias)
            .HasForeignKey(pc => pc.IdCategoriaProducto)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Mensajero - MensajeroNegocio
        modelBuilder.Entity<Mensajero>()
            .HasMany(m => m.MensajerosNegocios)
            .WithOne(mn => mn.Mensajero)
            .HasForeignKey(mn => mn.IdMensajero)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Negocio>()
          .HasMany(n => n.MensajerosNegocio)
          .WithOne(mn => mn.Negocio)
          .HasForeignKey(mn => mn.IdNegocio)
          .OnDelete(DeleteBehavior.Cascade);

        // Relación muchos a muchos entre Mensajero y Negocio
        modelBuilder.Entity<MensajeroNegocio>()
            .HasOne(mn => mn.Mensajero)
            .WithMany(m => m.MensajerosNegocios)
            .HasForeignKey(mn => mn.IdMensajero)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MensajeroNegocio>()
            .HasOne(mn => mn.Negocio)
            .WithMany(n => n.MensajerosNegocio)
            .HasForeignKey(mn => mn.IdNegocio)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NegocioCategoria>()
            .HasOne(nc => nc.Negocio)
            .WithMany(n => n.NegocioCategorias)
            .HasForeignKey(nc => nc.IdNegocio)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NegocioCategoria>()
            .HasOne(nc => nc.Categoria)
            .WithMany(c => c.NegociosCategorias)
            .HasForeignKey(nc => nc.IdCategoria)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.Cliente)
            .WithMany(c => c.Notificaciones)
            .HasForeignKey(n => n.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notificacion>()
          .HasOne(n => n.Pedido)
          .WithMany(p => p.Notificaciones)
          .HasForeignKey(n => n.IdPedido)
          .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.PlantillaNotificacion)
            .WithMany(pn => pn.Notificaciones)
            .HasForeignKey(n => n.IdPlantilla)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NegocioCliente>()
            .HasOne(nc => nc.Cliente)
            .WithMany(c => c.Negocios)
            .HasForeignKey(nc => nc.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NegocioCliente>()
            .HasOne(nc => nc.Negocio)
            .WithMany(n => n.Clientes)
            .HasForeignKey(nc => nc.IdNegocio)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cliente>()
            .HasMany(n => n.Notificaciones)
            .WithOne(n => n.Cliente)
            .HasForeignKey(n => n.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cliente>()
            .HasMany(n => n.Negocios)
            .WithOne(n => n.Cliente)
            .HasForeignKey(n => n.IdCliente)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.Notificaciones)
            .WithOne(n => n.Pedido)
            .HasForeignKey(n => n.IdPedido)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación PlantillaNotificacion - Notificacion
        modelBuilder.Entity<PlantillaNotificacion>()
            .HasMany(pn => pn.Notificaciones)
            .WithOne(n => n.PlantillaNotificacion)
            .HasForeignKey(n => n.IdPlantilla)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación de EstadoPedido con HistorialEstadoPedido
        modelBuilder.Entity<EstadoPedido>()
            .HasMany(e => e.HistorialEstadosPedido)
            .WithOne(hep => hep.EstadoPedido)
            .HasForeignKey(hep => hep.IdEstadoPedido)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EstadoPedido>()
        .HasOne(e => e.PlantillaNotificacion) // EstadoPedido tiene una PlantillaNotificacion
        .WithOne(p => p.EstadoPedido) // PlantillaNotificacion tiene un EstadoPedido
        .HasForeignKey<PlantillaNotificacion>(p => p.IdEstadoPedido) // Especificar la propiedad de la clave foránea
        .OnDelete(DeleteBehavior.SetNull);

        // Configuración de claves primarias
        modelBuilder.Entity<Cliente>()
            .HasKey(c => c.IdCliente);
        modelBuilder.Entity<Mensajero>()
            .HasKey(m => m.IdMensajero);
        modelBuilder.Entity<Negocio>()
            .HasKey(n => n.IdNegocio);
        modelBuilder.Entity<Telefono>()
            .HasKey(t => t.IdTelefono);
        modelBuilder.Entity<Direccion>()
            .HasKey(d => d.IdDireccion);
        modelBuilder.Entity<HorarioNegocio>()
            .HasKey(hn => hn.IdHorario);
        modelBuilder.Entity<CostoEnvio>()
            .HasKey(ce => ce.IdCostoEnvio);
        modelBuilder.Entity<CategoriaNegocio>()
            .HasKey(cn => cn.IdCategoria);
        modelBuilder.Entity<NegocioCategoria>()
            .HasKey(nc => nc.IdNegocioCategoria);
        modelBuilder.Entity<Producto>()
            .HasKey(p => p.IdProducto);
        modelBuilder.Entity<CategoriaProducto>()
            .HasKey(cp => cp.IdCategoriaProducto);
        modelBuilder.Entity<ProductoCategoria>()
            .HasKey(pc => pc.IdProductoCategoria);
        modelBuilder.Entity<MetodoPago>()
            .HasKey(mp => mp.IdMetodoPago);
        modelBuilder.Entity<EstadoPedido>()
            .HasKey(ep => ep.IdEstado);
        modelBuilder.Entity<Pedido>()
            .HasKey(p => p.IdPedido);
        modelBuilder.Entity<HistorialEstadoPedido>()
            .HasKey(hep => hep.IdHistorial);
        modelBuilder.Entity<PedidoProducto>()
            .HasKey(pp => pp.IdPedidoProducto);
        modelBuilder.Entity<PlantillaNotificacion>()
            .HasKey(pn => pn.IdPlantilla);
        modelBuilder.Entity<Notificacion>()
            .HasKey(n => n.IdNotificacion);
        modelBuilder.Entity<Rol>()
            .HasKey(r => r.IdRol);
        modelBuilder.Entity<Permiso>()
            .HasKey(per => per.IdPermiso);
        modelBuilder.Entity<ClienteRol>()
            .HasKey(cr => cr.IdClienteRol);
        modelBuilder.Entity<RolPermiso>()
            .HasKey(rp => rp.IdRolPermiso);
        modelBuilder.Entity<NegocioCliente>()
            .HasKey(nc => nc.IdNegocioCliente);
        modelBuilder.Entity<Imagen>()
            .HasKey(i => i.IdImagen);
        modelBuilder.Entity<MensajeroNegocio>()
            .HasKey(mn => mn.IdMensajeroNegocio);
    }

}
