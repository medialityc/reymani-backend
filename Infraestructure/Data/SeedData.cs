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
        new Rol { Codigo = "Administrador_Sistema", Descripcion = "Administrador del sistema" },
        new Rol { Codigo = "Administrador_Negocio", Descripcion = "Administrador del negocio" },
        new Rol { Codigo = "Cliente", Descripcion = "Cliente" },
        new Rol { Codigo = "Mensajero", Descripcion = "Mensajero" }
      );
      context.SaveChanges();
    }
  }

  public static void SeedPermisos(DBContext context)
  {
    if (!context.Permisos.Any())
    {
      context.Permisos.AddRange(
        new Permiso { Codigo = "Eliminar_Cliente", Descripcion = "Permiso para eliminar clientes" }
      );
      context.SaveChanges();
    }
  }
}
