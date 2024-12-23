using System;

namespace reymani_web_api.Infraestructure.Data;

public static class SeedData
{
  public static void SeedDatabase(DBContext context)
  {
    SeedRoles(context);
    SeedPermisos(context);
  }

  public static void SeedRoles(DBContext context)
  {
    if (!context.Roles.Any())
    {
      context.Roles.AddRange(
        new Rol { Nombre = "Administrador del Sistema", Descripcion = "Administrador del sistema saddsadsa" },
        new Rol { Nombre = "Administrador del Negocio", Descripcion = "Administrador del negocio dasdsa" },
        new Rol { Nombre = "Cliente", Descripcion = "Cliente dsa dsad sa" },
        new Rol { Nombre = "Mensajero", Descripcion = "Mensajero dasdsadsa" }
      );
      context.SaveChanges();
    }
  }

  public static void SeedPermisos(DBContext context)
  {
    if (!context.Permisos.Any())
    {
      context.Permisos.AddRange(
        new Permiso { Codigo = "Ver_Clientes", Descripcion = "Ver todos los clientes del sistema" },
        new Permiso { Codigo = "Cambiar_Contraseña", Descripcion = "Cambiar contraseña del cliente" },
        new Permiso { Codigo = "Eliminar_Cliente", Descripcion = "Eliminar cliente del sistema" },
        new Permiso { Codigo = "Ver_Cliente", Descripcion = "Ver informacion de cliente" },
        new Permiso { Codigo = "Actualizar_Cliente", Descripcion = "Actualizar informacion del cliente" },

        new Permiso { Codigo = "Ver_Permisos", Descripcion = "Ver todos los permisos del sistema" },
        new Permiso { Codigo = "Ver_Permiso", Descripcion = "Ver informacion del permiso" },

        new Permiso { Codigo = "Asignar_Rol_A_Cliente", Descripcion = "Asignar rol a los clientes del sistema" },
        new Permiso { Codigo = "Asignar_Permisos_A_Rol", Descripcion = "Asignar permisos a rol" },
        new Permiso { Codigo = "Crear_Rol", Descripcion = "Crear nuevo rol" },
        new Permiso { Codigo = "Eliminar_Rol", Descripcion = "Eliminar rol" },
        new Permiso { Codigo = "Ver_Roles", Descripcion = "Ver roles del sistema" },
        new Permiso { Codigo = "Ver_Rol", Descripcion = "Ver informacion del rol" },
        new Permiso { Codigo = "Actualizar_Rol", Descripcion = "Actualizar rol" }
      );
      context.SaveChanges();
    }
  }
}
