using System;
using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Infraestructure.Repositories;

public class TelefonoRepository : ITelefonoRepository
{
  private readonly DBContext _context;

  public TelefonoRepository(DBContext context)
  {
    _context = context;
  }

  public async Task<Telefono> AddAsync(Telefono telefono)
  {
    _context.Set<Telefono>().Add(telefono);
    await _context.SaveChangesAsync();
    return telefono;
  }

  public async Task<Telefono?> GetByIdAsync(Guid id)
  {
    return await _context.Set<Telefono>().FindAsync(id);
  }

  public async Task<IEnumerable<Telefono>> GetAllAsync()
  {
    return await _context.Set<Telefono>().ToListAsync();
  }

  public async Task UpdateAsync(Telefono telefono)
  {
    _context.Set<Telefono>().Update(telefono);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(Guid id)
  {
    var telefono = await _context.Set<Telefono>().FindAsync(id);
    if (telefono != null)
    {
      _context.Set<Telefono>().Remove(telefono);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<Telefono?> GetByNumeroAndEntidadAsync(string numero, Guid idEntidad)
  {
    return await _context.Set<Telefono>()
      .FirstOrDefaultAsync(t => t.NumeroTelefono == numero && t.IdEntidad == idEntidad);
  }

  public async Task<IEnumerable<Telefono>> GetAllByIdEntidadAsync(Guid idEntidad)
  {
    return await _context.Set<Telefono>().Where(t => t.IdEntidad == idEntidad).ToListAsync();
  }
}
