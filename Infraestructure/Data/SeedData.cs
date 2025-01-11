using System;

namespace reymani_web_api.Infraestructure.Data;

public static class SeedData
{
  public static void SeedDatabase(DBContext context)
  {
    SeedRoles(context);
    SeedPermisos(context);
    SeedRolPermisos(context);
  }

  public static void SeedRoles(DBContext context)
  {
    if (!context.Roles.Any())
    {
      context.Roles.AddRange(
        new Rol { IdRol = Guid.NewGuid(), Nombre = "Administrador del Sistema", Descripcion = "Administrador del sistema" },
        new Rol { IdRol = Guid.NewGuid(), Nombre = "Administrador del Negocio", Descripcion = "Administrador del negocio" },
        new Rol { IdRol = Guid.NewGuid(), Nombre = "Cliente", Descripcion = "Cliente" },
        new Rol { IdRol = Guid.NewGuid(), Nombre = "Mensajero", Descripcion = "Mensajero" }
      );
      context.SaveChanges();
    }
  }

  public static void SeedPermisos(DBContext context)
  {
    if (!context.Permisos.Any())
    {
      context.Permisos.AddRange(
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Clientes", Descripcion = "Ver todos los clientes del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Cambiar_Contraseña", Descripcion = "Cambiar contraseña del cliente" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Cliente", Descripcion = "Eliminar cliente del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Cliente", Descripcion = "Ver informacion de cliente" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Cliente", Descripcion = "Actualizar informacion del cliente" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Permisos", Descripcion = "Ver todos los permisos del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Permiso", Descripcion = "Ver informacion del permiso" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Asignar_Roles_A_Cliente", Descripcion = "Asignar rol a los clientes del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Asignar_Permisos_A_Rol", Descripcion = "Asignar permisos a rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Rol", Descripcion = "Crear nuevo rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Rol", Descripcion = "Eliminar rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Roles", Descripcion = "Ver roles del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Rol", Descripcion = "Ver informacion del rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Rol", Descripcion = "Actualizar rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Permisos_Rol", Descripcion = "Ver permisos de un rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Cambiar_Estado_Cliente", Descripcion = "Cambiar estado del cliente" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Negocio", Descripcion = "Crear nuevo negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Negocios", Descripcion = "Ver todos los negocios del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Negocio", Descripcion = "Ver informacion del negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Negocio", Descripcion = "Eliminar negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Negocio", Descripcion = "Actualizar informacion del negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Telefono", Descripcion = "Añadir nuevo telefono" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Telefonos", Descripcion = "Ver todos los telefonos del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Telefono", Descripcion = "Ver informacion del telefono" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Telefono", Descripcion = "Eliminar telefono" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Telefono", Descripcion = "Actualizar informacion del telefono" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Telefonos_Entidad", Descripcion = "Ver telefonos de una entidad" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Direccion", Descripcion = "Añadir nueva direccion" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Direcciones", Descripcion = "Ver todas las direcciones del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Direccion", Descripcion = "Ver informacion de la direccion" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Direccion", Descripcion = "Eliminar direccion" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Direccion", Descripcion = "Actualizar informacion de la direccion" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Direcciones_Entidad", Descripcion = "Ver direcciones de una entidad" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Categoria_Negocio", Descripcion = "Crear nueva categoria de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Categorias_Negocio", Descripcion = "Ver todas las categorias de negocio del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Categoria_Negocio", Descripcion = "Ver informacion de la categoria de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Categoria_Negocio", Descripcion = "Eliminar categoria de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Categoria_Negocio", Descripcion = "Actualizar informacion de la categoria de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Costo_Envio", Descripcion = "Crear nuevo costo de envio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Costos_Envio", Descripcion = "Ver todos los costos de envio del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Costo_Envio", Descripcion = "Ver informacion del costo de envio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Costo_Envio", Descripcion = "Eliminar costo de envio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Costo_Envio", Descripcion = "Actualizar informacion del costo de envio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Costos_Envio_Negocio", Descripcion = "Ver costos de envio de un negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Asignar_Categorias_Negocio_A_Negocio", Descripcion = "Asignar categorias de negocio a un negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Categorias_Negocio_Negocio", Descripcion = "Ver categorias de negocio de un negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Horario_Negocio", Descripcion = "Crear nuevo horario de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Horarios_Negocio", Descripcion = "Ver todos los horarios de negocio del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Horario_Negocio", Descripcion = "Ver informacion del horario de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Horario_Negocio", Descripcion = "Eliminar horario de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Horario_Negocio", Descripcion = "Actualizar informacion del horario de negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Horarios_Negocio_Negocio", Descripcion = "Ver horarios de negocio de un negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Negocio_Cliente", Descripcion = "Subscribirse a un negocio" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Negocio_Cliente", Descripcion = "Desubscribirse de un negocio" }

      );
      context.SaveChanges();
    }
  }

  public static void SeedRolPermisos(DBContext context)
  {
    var adminRol = context.Roles.FirstOrDefault(r => r.Nombre == "Administrador del Sistema");
    var clienteRol = context.Roles.FirstOrDefault(r => r.Nombre == "Cliente");

    if (adminRol != null && clienteRol != null)
    {
      var permisos = context.Permisos.ToList();
      var clientePermisos = permisos.Where(p => p.Codigo.Contains("Cliente")).ToList();

      foreach (var permiso in permisos)
      {
        context.RolesPermisos.Add(new RolPermiso { IdRolPermiso = Guid.NewGuid(), IdRol = adminRol.IdRol, IdPermiso = permiso.IdPermiso });
      }

      foreach (var permiso in clientePermisos)
      {
        context.RolesPermisos.Add(new RolPermiso { IdRolPermiso = Guid.NewGuid(), IdRol = clienteRol.IdRol, IdPermiso = permiso.IdPermiso });
      }

      context.SaveChanges();
    }
  }
}
