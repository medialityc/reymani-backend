
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Infraestructure.Repositories;


public class ClienteRepository : IClienteRepository
{
  private readonly DBContext _context;

  public ClienteRepository(DBContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Cliente>> GetAllAsync()
  {
    return await _context.Clientes.ToListAsync();
  }

  public async Task<Cliente?> GetByIdAsync(Guid id)
  {
    return await _context.Clientes.FindAsync(id);
  }

  public async Task AddAsync(Cliente cliente)
  {
    await _context.Clientes.AddAsync(cliente);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateAsync(Cliente cliente)
  {
    _context.Clientes.Update(cliente);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(Cliente cliente)
  {
    _context.Clientes.Remove(cliente);
    await _context.SaveChangesAsync();
  }

  public async Task<Cliente?> GetClienteByUsernameOrPhoneAsync(string usernameOrPhone)
  {
    var cliente = await _context.Clientes
        .FirstOrDefaultAsync(c => c.Username == usernameOrPhone ||
            _context.Telefonos.Any(t => t.NumeroTelefono == usernameOrPhone &&
                t.IdEntidad == c.IdCliente &&
                t.TipoEntidad == EntitiesTypes.Cliente.ToString()));

    return cliente ?? null;
  }

  public async Task<string[]> GetCodigosRolesClienteAsync(Guid id)
  {
    var roles = await _context.Clientes
        .Where(c => c.IdCliente == id)
        .SelectMany(c => c.Roles.Select(cr => cr.Rol!.Codigo)) // Navega de Cliente -> ClienteRol -> Rol -> Codigo
        .ToArrayAsync();

    return roles;
  }

  public async Task<string[]> GetCodigosPermisosClienteAsync(Guid id)
  {
    var permisos = await _context.Clientes
        .Where(c => c.IdCliente == id)
        .SelectMany(c => c.Roles.SelectMany(cr => cr.Rol!.Permisos.Select(rp => rp.Permiso!.Codigo))) // Navega de Cliente -> ClienteRol -> Rol -> RolPermiso -> Permiso -> Codigo
        .ToArrayAsync();

    return permisos;
  }
}
