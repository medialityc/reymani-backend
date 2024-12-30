using Microsoft.EntityFrameworkCore;


namespace reymani_web_api.Infraestructure.Repositories
{
  public class RolRepository : IRolRepository
  {
    private readonly DBContext _context;

    public RolRepository(DBContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Rol>> GetAllAsync()
    {
      return await _context.Roles.ToListAsync();
    }

    public async Task<Rol?> GetByIdAsync(Guid id)
    {
      return await _context.Roles.FindAsync(id);
    }

    public async Task AddAsync(Rol rol, IEnumerable<Guid> permisoIds)
    {
      await _context.Roles.AddAsync(rol);
      foreach (var permisoId in permisoIds)
      {
        var permiso = await _context.Permisos.FindAsync(permisoId);
        if (permiso != null)
        {
          rol.Permisos.Add(new RolPermiso { IdRol = rol.IdRol, IdPermiso = permisoId });
        }
      }
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rol rol, IEnumerable<Guid> permisoIds)
    {
      var existingRol = await _context.Roles.Include(r => r.Permisos).FirstOrDefaultAsync(r => r.IdRol == rol.IdRol);
      if (existingRol != null)
      {
        existingRol.Nombre = rol.Nombre;
        existingRol.Descripcion = rol.Descripcion;

        // Update permissions
        existingRol.Permisos.Clear();
        foreach (var permisoId in permisoIds)
        {
          existingRol.Permisos.Add(new RolPermiso { IdRol = rol.IdRol, IdPermiso = permisoId });
        }

        _context.Roles.Update(existingRol);
        await _context.SaveChangesAsync();
      }
    }

    public async Task DeleteAsync(Rol rol)
    {
      _context.Roles.Remove(rol);
      await _context.SaveChangesAsync();
    }

    public async Task AssignPermissionsAsync(Guid rolId, IEnumerable<Guid> permisoIds)
    {
      var rol = await _context.Roles.Include(r => r.Permisos).FirstOrDefaultAsync(r => r.IdRol == rolId);
      if (rol == null) throw new Exception("Rol no encontrado.");

      rol.Permisos.Clear();
      foreach (var permisoId in permisoIds)
      {
        var permiso = await _context.Permisos.FindAsync(permisoId);
        if (permiso != null)
        {
          rol.Permisos.Add(new RolPermiso { IdRol = rolId, IdPermiso = permisoId });
        }
      }
      await _context.SaveChangesAsync();
    }

    public async Task<string[]> GetCodigosPermisosRolAsync(Guid id)
    {
      var permisos = await _context.Roles
          .Where(r => r.IdRol == id)
          .SelectMany(r => r.Permisos.Select(rp => rp.Permiso!.Codigo)) // Navega de Rol -> RolPermiso -> Permiso -> Codigo
          .ToArrayAsync();

      return permisos;
    }

    public async Task<bool> RolNameExistsAsync(string nombre)
    {
      return await _context.Roles.AnyAsync(r => r.Nombre.ToLower() == nombre.ToLower());
    }

    public async Task<Permiso[]> GetPermisosRolAsync(Guid id)
    {
      var permisos = await _context.Roles
          .Where(r => r.IdRol == id)
          .SelectMany(r => r.Permisos.Select(rp => rp.Permiso!)) // Navega de Rol -> RolPermiso -> Permiso
          .ToArrayAsync();

      return permisos;
    }
  }
}
