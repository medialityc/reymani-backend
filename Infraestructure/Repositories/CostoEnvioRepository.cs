using System;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Infraestructure.Repositories
{
  public class CostoEnvioRepository : ICostoEnvioRepository
  {
    private readonly DBContext _context;

    public CostoEnvioRepository(DBContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<CostoEnvio>> GetAllAsync()
    {
      return await _context.CostosEnvios.ToListAsync();
    }

    public async Task<CostoEnvio?> GetByIdAsync(Guid id)
    {
      return await _context.CostosEnvios.FindAsync(id);
    }

    public async Task AddAsync(CostoEnvio costoEnvio)
    {
      await _context.CostosEnvios.AddAsync(costoEnvio);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CostoEnvio costoEnvio)
    {
      _context.CostosEnvios.Update(costoEnvio);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CostoEnvio costoEnvio)
    {
      _context.CostosEnvios.Remove(costoEnvio);
      await _context.SaveChangesAsync();
    }
  }
}
