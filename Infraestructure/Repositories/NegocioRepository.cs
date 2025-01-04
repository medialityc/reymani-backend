using Microsoft.EntityFrameworkCore;


namespace reymani_web_api.Infraestructure.Repositories;

public class NegocioRepository : INegocioRepository
{
  private readonly DBContext _context;

  public NegocioRepository(DBContext context)
  {
    _context = context;
  }

  public async Task<Negocio> AddAsync(Negocio negocio)
  {
    _context.Set<Negocio>().Add(negocio);
    await _context.SaveChangesAsync();
    return negocio;
  }

  public async Task<Negocio?> GetByIdAsync(Guid id)
  {
    return await _context.Set<Negocio>().FindAsync(id);
  }

  public async Task<IEnumerable<Negocio>> GetAllAsync()
  {
    return await _context.Set<Negocio>().ToListAsync();
  }

  public async Task UpdateAsync(Negocio negocio)
  {
    _context.Set<Negocio>().Update(negocio);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(Guid id)
  {
    var negocio = await _context.Set<Negocio>().FindAsync(id);
    if (negocio != null)
    {
      _context.Set<Negocio>().Remove(negocio);
      await _context.SaveChangesAsync();
    }
  }
}
