using System;
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
}
