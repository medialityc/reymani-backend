using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace reymani_web_api.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasNegocios",
                columns: table => new
                {
                    IdCategoria = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasNegocios", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasProducto",
                columns: table => new
                {
                    IdCategoriaProducto = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasProducto", x => x.IdCategoriaProducto);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroCarnet = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Direcciones",
                columns: table => new
                {
                    IdDireccion = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoEntidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IdEntidad = table.Column<Guid>(type: "uuid", nullable: false),
                    DireccionEntidad = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Municipio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Provincia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Longitud = table.Column<double>(type: "double precision", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direcciones", x => x.IdDireccion);
                });

            migrationBuilder.CreateTable(
                name: "EstadosPedido",
                columns: table => new
                {
                    IdEstado = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    TiempoDuracionEstimado = table.Column<int>(type: "integer", nullable: false),
                    IdPlantillaNotificacion = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosPedido", x => x.IdEstado);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    IdImagen = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoEntidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IdEntidad = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.IdImagen);
                });

            migrationBuilder.CreateTable(
                name: "Mensajeros",
                columns: table => new
                {
                    IdMensajero = table.Column<Guid>(type: "uuid", nullable: false),
                    Vehiculo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajeros", x => x.IdMensajero);
                });

            migrationBuilder.CreateTable(
                name: "MetodosPago",
                columns: table => new
                {
                    IdMetodoPago = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoEntidad = table.Column<string>(type: "text", nullable: false),
                    IdEntidad = table.Column<Guid>(type: "uuid", nullable: false),
                    Proveedor = table.Column<string>(type: "text", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Dato1 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Dato2 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Dato3 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosPago", x => x.IdMetodoPago);
                });

            migrationBuilder.CreateTable(
                name: "Negocios",
                columns: table => new
                {
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    EntregaDomicilio = table.Column<bool>(type: "boolean", nullable: false),
                    URLImagenPrincipal = table.Column<string>(type: "text", nullable: true),
                    URLImagenLogo = table.Column<string>(type: "text", nullable: true),
                    URLImagenBanner = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negocios", x => x.IdNegocio);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    IdPermiso = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.IdPermiso);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Telefonos",
                columns: table => new
                {
                    IdTelefono = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoEntidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IdEntidad = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroTelefono = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonos", x => x.IdTelefono);
                });

            migrationBuilder.CreateTable(
                name: "PlantillasNotificacion",
                columns: table => new
                {
                    IdPlantilla = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    IdEstadoPedido = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillasNotificacion", x => x.IdPlantilla);
                    table.ForeignKey(
                        name: "FK_PlantillasNotificacion_EstadosPedido_IdEstadoPedido",
                        column: x => x.IdEstadoPedido,
                        principalTable: "EstadosPedido",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CostosEnvios",
                columns: table => new
                {
                    IdCostoEnvio = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    DistanciaMaxKm = table.Column<int>(type: "integer", nullable: false),
                    Costo = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostosEnvios", x => x.IdCostoEnvio);
                    table.ForeignKey(
                        name: "FK_CostosEnvios_Negocios_IdNegocio",
                        column: x => x.IdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HorariosNegocios",
                columns: table => new
                {
                    IdHorario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    Dia = table.Column<int>(type: "integer", nullable: false),
                    HoraApertura = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraCierre = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosNegocios", x => x.IdHorario);
                    table.ForeignKey(
                        name: "FK_HorariosNegocios_Negocios_IdNegocio",
                        column: x => x.IdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MensajeroNegocios",
                columns: table => new
                {
                    IdMensajeroNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    IdMensajero = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaAsociacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensajeroNegocios", x => x.IdMensajeroNegocio);
                    table.ForeignKey(
                        name: "FK_MensajeroNegocios_Mensajeros_IdMensajero",
                        column: x => x.IdMensajero,
                        principalTable: "Mensajeros",
                        principalColumn: "IdMensajero",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MensajeroNegocios_Negocios_IdNegocio",
                        column: x => x.IdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NegocioCategorias",
                columns: table => new
                {
                    IdNegocioCategoria = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    IdCategoria = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegocioCategorias", x => x.IdNegocioCategoria);
                    table.ForeignKey(
                        name: "FK_NegocioCategorias_CategoriasNegocios_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriasNegocios",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NegocioCategorias_Negocios_IdNegocio",
                        column: x => x.IdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NegociosUsuarios",
                columns: table => new
                {
                    IdNegocioUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NegociosUsuarios", x => x.IdNegocioUsuario);
                    table.ForeignKey(
                        name: "FK_NegociosUsuarios_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NegociosUsuarios_Negocios_IdNegocio",
                        column: x => x.IdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdMensajero = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    DireccionEntrega = table.Column<Guid>(type: "uuid", nullable: false),
                    IdMetodoPago = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfirmacionPago = table.Column<bool>(type: "boolean", nullable: false),
                    IdEstado = table.Column<Guid>(type: "uuid", nullable: false),
                    Total = table.Column<double>(type: "double precision", nullable: false),
                    CostoEnvio = table.Column<double>(type: "double precision", nullable: false),
                    SubtotalProductos = table.Column<double>(type: "double precision", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFinalizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Valoracion = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    cerrado = table.Column<bool>(type: "boolean", nullable: false),
                    Descuento = table.Column<double>(type: "double precision", nullable: false),
                    DescripcionDescuento = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MensajeroIdMensajero = table.Column<Guid>(type: "uuid", nullable: true),
                    NegocioIdNegocio = table.Column<Guid>(type: "uuid", nullable: true),
                    DireccionIdDireccion = table.Column<Guid>(type: "uuid", nullable: true),
                    MetodoPagoIdMetodoPago = table.Column<Guid>(type: "uuid", nullable: true),
                    EstadoPedidoIdEstado = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Direcciones_DireccionIdDireccion",
                        column: x => x.DireccionIdDireccion,
                        principalTable: "Direcciones",
                        principalColumn: "IdDireccion");
                    table.ForeignKey(
                        name: "FK_Pedidos_EstadosPedido_EstadoPedidoIdEstado",
                        column: x => x.EstadoPedidoIdEstado,
                        principalTable: "EstadosPedido",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK_Pedidos_EstadosPedido_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "EstadosPedido",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pedidos_Mensajeros_MensajeroIdMensajero",
                        column: x => x.MensajeroIdMensajero,
                        principalTable: "Mensajeros",
                        principalColumn: "IdMensajero");
                    table.ForeignKey(
                        name: "FK_Pedidos_MetodosPago_MetodoPagoIdMetodoPago",
                        column: x => x.MetodoPagoIdMetodoPago,
                        principalTable: "MetodosPago",
                        principalColumn: "IdMetodoPago");
                    table.ForeignKey(
                        name: "FK_Pedidos_Negocios_NegocioIdNegocio",
                        column: x => x.NegocioIdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdProducto = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Precio = table.Column<double>(type: "double precision", nullable: false),
                    Stock = table.Column<double>(type: "double precision", nullable: false),
                    StockCongelado = table.Column<double>(type: "double precision", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    URLImagen = table.Column<string>(type: "text", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Negocios_IdNegocio",
                        column: x => x.IdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRoles",
                columns: table => new
                {
                    IdUsuarioRol = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => x.IdUsuarioRol);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolesPermisos",
                columns: table => new
                {
                    IdRolPermiso = table.Column<Guid>(type: "uuid", nullable: false),
                    IdRol = table.Column<Guid>(type: "uuid", nullable: false),
                    IdPermiso = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesPermisos", x => x.IdRolPermiso);
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Permisos_IdPermiso",
                        column: x => x.IdPermiso,
                        principalTable: "Permisos",
                        principalColumn: "IdPermiso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstadosPedido",
                columns: table => new
                {
                    IdHistorial = table.Column<Guid>(type: "uuid", nullable: false),
                    IdPedido = table.Column<Guid>(type: "uuid", nullable: false),
                    IdEstadoPedido = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstadosPedido", x => x.IdHistorial);
                    table.ForeignKey(
                        name: "FK_HistorialEstadosPedido_EstadosPedido_IdEstadoPedido",
                        column: x => x.IdEstadoPedido,
                        principalTable: "EstadosPedido",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialEstadosPedido_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificaciones",
                columns: table => new
                {
                    IdNotificacion = table.Column<Guid>(type: "uuid", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uuid", nullable: false),
                    IdPedido = table.Column<Guid>(type: "uuid", nullable: false),
                    IdNegocio = table.Column<Guid>(type: "uuid", nullable: false),
                    NegocioIdNegocio = table.Column<Guid>(type: "uuid", nullable: true),
                    IdPlantilla = table.Column<Guid>(type: "uuid", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificaciones", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificaciones_Negocios_NegocioIdNegocio",
                        column: x => x.NegocioIdNegocio,
                        principalTable: "Negocios",
                        principalColumn: "IdNegocio");
                    table.ForeignKey(
                        name: "FK_Notificaciones_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificaciones_PlantillasNotificacion_IdPlantilla",
                        column: x => x.IdPlantilla,
                        principalTable: "PlantillasNotificacion",
                        principalColumn: "IdPlantilla",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidosProductos",
                columns: table => new
                {
                    IdPedidoProducto = table.Column<Guid>(type: "uuid", nullable: false),
                    IdPedido = table.Column<Guid>(type: "uuid", nullable: false),
                    IdProducto = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductoIdProducto = table.Column<Guid>(type: "uuid", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: false),
                    Subtotal = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosProductos", x => x.IdPedidoProducto);
                    table.ForeignKey(
                        name: "FK_PedidosProductos_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosProductos_Productos_ProductoIdProducto",
                        column: x => x.ProductoIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductosCategorias",
                columns: table => new
                {
                    IdProductoCategoria = table.Column<Guid>(type: "uuid", nullable: false),
                    IdProducto = table.Column<Guid>(type: "uuid", nullable: false),
                    IdCategoriaProducto = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosCategorias", x => x.IdProductoCategoria);
                    table.ForeignKey(
                        name: "FK_ProductosCategorias_CategoriasProducto_IdCategoriaProducto",
                        column: x => x.IdCategoriaProducto,
                        principalTable: "CategoriasProducto",
                        principalColumn: "IdCategoriaProducto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductosCategorias_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_IdUsuario",
                table: "UsuariosRoles",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_IdRol",
                table: "UsuariosRoles",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_CostosEnvios_IdNegocio",
                table: "CostosEnvios",
                column: "IdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstadosPedido_IdEstadoPedido",
                table: "HistorialEstadosPedido",
                column: "IdEstadoPedido");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstadosPedido_IdPedido",
                table: "HistorialEstadosPedido",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosNegocios_IdNegocio",
                table: "HorariosNegocios",
                column: "IdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_MensajeroNegocios_IdMensajero",
                table: "MensajeroNegocios",
                column: "IdMensajero");

            migrationBuilder.CreateIndex(
                name: "IX_MensajeroNegocios_IdNegocio",
                table: "MensajeroNegocios",
                column: "IdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_NegocioCategorias_IdCategoria",
                table: "NegocioCategorias",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_NegocioCategorias_IdNegocio",
                table: "NegocioCategorias",
                column: "IdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_NegociosUsuarios_IdUsuario",
                table: "NegociosUsuarios",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_NegociosUsuarios_IdNegocio",
                table: "NegociosUsuarios",
                column: "IdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_IdUsuario",
                table: "Notificaciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_IdPedido",
                table: "Notificaciones",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_IdPlantilla",
                table: "Notificaciones",
                column: "IdPlantilla");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_NegocioIdNegocio",
                table: "Notificaciones",
                column: "NegocioIdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DireccionIdDireccion",
                table: "Pedidos",
                column: "DireccionIdDireccion");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EstadoPedidoIdEstado",
                table: "Pedidos",
                column: "EstadoPedidoIdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdUsuario",
                table: "Pedidos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdEstado",
                table: "Pedidos",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_MensajeroIdMensajero",
                table: "Pedidos",
                column: "MensajeroIdMensajero");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_MetodoPagoIdMetodoPago",
                table: "Pedidos",
                column: "MetodoPagoIdMetodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_NegocioIdNegocio",
                table: "Pedidos",
                column: "NegocioIdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProductos_IdPedido",
                table: "PedidosProductos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProductos_ProductoIdProducto",
                table: "PedidosProductos",
                column: "ProductoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillasNotificacion_IdEstadoPedido",
                table: "PlantillasNotificacion",
                column: "IdEstadoPedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IdNegocio",
                table: "Productos",
                column: "IdNegocio");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosCategorias_IdCategoriaProducto",
                table: "ProductosCategorias",
                column: "IdCategoriaProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosCategorias_IdProducto",
                table: "ProductosCategorias",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermisos_IdPermiso",
                table: "RolesPermisos",
                column: "IdPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermisos_IdRol",
                table: "RolesPermisos",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosRoles");

            migrationBuilder.DropTable(
                name: "CostosEnvios");

            migrationBuilder.DropTable(
                name: "HistorialEstadosPedido");

            migrationBuilder.DropTable(
                name: "HorariosNegocios");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "MensajeroNegocios");

            migrationBuilder.DropTable(
                name: "NegocioCategorias");

            migrationBuilder.DropTable(
                name: "NegociosUsuarios");

            migrationBuilder.DropTable(
                name: "Notificaciones");

            migrationBuilder.DropTable(
                name: "PedidosProductos");

            migrationBuilder.DropTable(
                name: "ProductosCategorias");

            migrationBuilder.DropTable(
                name: "RolesPermisos");

            migrationBuilder.DropTable(
                name: "Telefonos");

            migrationBuilder.DropTable(
                name: "CategoriasNegocios");

            migrationBuilder.DropTable(
                name: "PlantillasNotificacion");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "CategoriasProducto");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Direcciones");

            migrationBuilder.DropTable(
                name: "EstadosPedido");

            migrationBuilder.DropTable(
                name: "Mensajeros");

            migrationBuilder.DropTable(
                name: "MetodosPago");

            migrationBuilder.DropTable(
                name: "Negocios");
        }
    }
}
