
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Infraestructure.Repositories;


public class UsuarioRepository : IUsuarioRepository
{
  private readonly DBContext _context;

  public UsuarioRepository(DBContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Usuario>> GetAllAsync()
  {
    return await _context.Usuarios.ToListAsync();
  }

  public async Task<Usuario?> GetByIdAsync(Guid id)
  {
    return await _context.Usuarios.FindAsync(id);
  }

  public async Task AddAsync(Usuario Usuario)
  {
    await _context.Usuarios.AddAsync(Usuario);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Usuario Usuario)
  {
    _context.Usuarios.Update(Usuario);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(Usuario Usuario)
  {
    _context.Usuarios.Remove(Usuario);
    await _context.SaveChangesAsync();
  }

  public async Task<Usuario?> GetUsuarioByUsernameOrPhoneAsync(string usernameOrPhone)
  {
    var Usuario = await _context.Usuarios
        .FirstOrDefaultAsync(c => c.Username == usernameOrPhone ||
            _context.Telefonos.Any(t => t.NumeroTelefono == usernameOrPhone &&
                t.IdEntidad == c.IdUsuario &&
                t.TipoEntidad == EntitiesTypes.Usuario.ToString()));

    return Usuario ?? null;
  }

  public async Task<string[]> GetIdRolesUsuarioAsync(Guid id)
  {
    var roles = await _context.Usuarios
        .Where(c => c.IdUsuario == id)
        .SelectMany(c => c.Roles.Select(cr => cr.Rol!.IdRol.ToString())) // Navega de Usuario -> UsuarioRol -> Rol -> Nombre
        .ToArrayAsync();

    return roles;
  }

  public async Task<List<string>> GetPermissionsAsync(Guid UsuarioId)
  {
    var permisos = await _context.UsuariosRoles
             .Where(cr => cr.IdUsuario == UsuarioId && cr.Rol != null)
             .SelectMany(cr => cr.Rol!.Permisos)
             .Where(rp => rp.Permiso != null)
             .Select(rp => rp.Permiso!.Codigo)
             .Distinct() // Evita duplicados
             .ToListAsync();

    return permisos;
  }
}
