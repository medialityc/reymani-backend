using Microsoft.EntityFrameworkCore;


namespace reymani_web_api.Infraestructure.Repositories;

public class MetodoPagoRepository : IMetodoPagoRepository
{
  private readonly DBContext _context;

  public MetodoPagoRepository(DBContext context)
  {
    _context = context;
  }

  public async Task<MetodoPago> AddAsync(MetodoPago metodoPago)
  {
    _context.Set<MetodoPago>().Add(metodoPago);
    await _context.SaveChangesAsync();
    return metodoPago;
  }

  public async Task<MetodoPago?> GetByIdAsync(Guid id)
  {
    return await _context.Set<MetodoPago>().FindAsync(id);
  }

  public async Task<IEnumerable<MetodoPago>> GetAllAsync()
  {
    return await _context.Set<MetodoPago>().ToListAsync();
  }

  public async Task UpdateAsync(MetodoPago metodoPago)
  {
    _context.Set<MetodoPago>().Update(metodoPago);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(Guid id)
  {
    var metodoPago = await _context.Set<MetodoPago>().FindAsync(id);
    if (metodoPago != null)
    {
      _context.Set<MetodoPago>().Remove(metodoPago);
      await _context.SaveChangesAsync();
    }
  }

  public async Task<IEnumerable<MetodoPago>> GetAllByIdEntidadAsync(Guid idEntidad)
  {
    return await _context.Set<MetodoPago>().Where(mp => mp.IdEntidad == idEntidad).ToListAsync();
  }

  public async Task<bool> ExistsByEntidadAndProveedorAsync(Guid idEntidad, string proveedor)
  {
    return await _context.Set<MetodoPago>().AnyAsync(mp => mp.IdEntidad == idEntidad && mp.Proveedor == proveedor);
  }
}
