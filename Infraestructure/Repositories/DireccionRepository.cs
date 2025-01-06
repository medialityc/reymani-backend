using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Infraestructure.Repositories
{
  public class DireccionRepository : IDireccionRepository
  {
    private readonly DBContext _context;

    public DireccionRepository(DBContext context)
    {
      _context = context;
    }

    public async Task<Direccion> AddAsync(Direccion direccion)
    {
      _context.Set<Direccion>().Add(direccion);
      await _context.SaveChangesAsync();
      return direccion;
    }

    public async Task<Direccion?> GetByIdAsync(Guid id)
    {
      return await _context.Set<Direccion>().FindAsync(id);
    }

    public async Task<IEnumerable<Direccion>> GetAllAsync()
    {
      return await _context.Set<Direccion>().ToListAsync();
    }

    public async Task UpdateAsync(Direccion direccion)
    {
      _context.Set<Direccion>().Update(direccion);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
      var direccion = await _context.Set<Direccion>().FindAsync(id);
      if (direccion != null)
      {
        _context.Set<Direccion>().Remove(direccion);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<IEnumerable<Direccion>> GetAllByIdEntidadAsync(Guid idEntidad)
    {
      return await _context.Set<Direccion>().Where(d => d.IdEntidad == idEntidad).ToListAsync();
    }
  }
}
