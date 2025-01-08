using System;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Infraestructure.Repositories
{
  public class CategoriaNegocioRepository : ICategoriaNegocioRepository
  {
    private readonly DBContext _context;

    public CategoriaNegocioRepository(DBContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<CategoriaNegocio>> GetAllAsync()
    {
      return await _context.CategoriasNegocios.ToListAsync();
    }

    public async Task<CategoriaNegocio?> GetByIdAsync(Guid id)
    {
      return await _context.CategoriasNegocios.FindAsync(id);
    }

    public async Task<CategoriaNegocio?> GetByNameAsync(string nombre)
    {
      return await _context.CategoriasNegocios.FirstOrDefaultAsync(c => c.Nombre == nombre);
    }

    public async Task AddAsync(CategoriaNegocio categoriaNegocio)
    {
      await _context.CategoriasNegocios.AddAsync(categoriaNegocio);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoriaNegocio categoriaNegocio)
    {
      var existingEntity = await _context.CategoriasNegocios.FindAsync(categoriaNegocio.IdCategoria);
      if (existingEntity != null)
      {
        _context.Entry(existingEntity).State = EntityState.Detached;
      }
      _context.CategoriasNegocios.Update(categoriaNegocio);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CategoriaNegocio categoriaNegocio)
    {
      _context.CategoriasNegocios.Remove(categoriaNegocio);
      await _context.SaveChangesAsync();
    }
  }
}
