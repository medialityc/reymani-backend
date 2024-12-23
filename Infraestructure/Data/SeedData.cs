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
        new Rol { Nombre = "Mensajero", Descripcion = "Mensajero dasdsadsa " }
      );
      context.SaveChanges();
    }
  }

  public static void SeedPermisos(DBContext context)
  {
    if (!context.Permisos.Any())
    {
      context.Permisos.AddRange(
        new Permiso { Codigo = "Acceso_Backoffice", Descripcion = "Acceso al backoffice" }
      );
      context.SaveChanges();
    }
  }
}
