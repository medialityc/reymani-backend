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
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Asignar_Rol_A_Cliente", Descripcion = "Asignar rol a los clientes del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Asignar_Permisos_A_Rol", Descripcion = "Asignar permisos a rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Crear_Rol", Descripcion = "Crear nuevo rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Eliminar_Rol", Descripcion = "Eliminar rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Roles", Descripcion = "Ver roles del sistema" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Ver_Rol", Descripcion = "Ver informacion del rol" },
        new Permiso { IdPermiso = Guid.NewGuid(), Codigo = "Actualizar_Rol", Descripcion = "Actualizar rol" }
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
