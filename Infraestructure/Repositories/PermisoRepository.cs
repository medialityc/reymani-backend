using Microsoft.EntityFrameworkCore;


namespace reymani_web_api.Infraestructure.Repositories
{
  public class PermisoRepository : IPermisoRepository
  {
    private readonly DBContext _context;

    public PermisoRepository(DBContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Permiso>> GetAllAsync()
    {
      return await _context.Permisos.ToListAsync();
    }

    public async Task<Permiso?> GetByIdAsync(Guid id)
    {
      return await _context.Permisos.FindAsync(id);
    }

    public async Task AddAsync(Permiso permiso)
    {
      await _context.Permisos.AddAsync(permiso);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Permiso permiso)
    {
      _context.Permisos.Update(permiso);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Permiso permiso)
    {
      _context.Permisos.Remove(permiso);
      await _context.SaveChangesAsync();

    }
  }
}
