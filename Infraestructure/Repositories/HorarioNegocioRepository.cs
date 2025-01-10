using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Infraestructure.Repositories
{
  public class HorarioNegocioRepository : IHorarioNegocioRepository
  {
    private readonly DBContext _context;

    public HorarioNegocioRepository(DBContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<HorarioNegocio>> GetAllAsync()
    {
      return await _context.HorariosNegocios.ToListAsync();
    }

    public async Task<HorarioNegocio?> GetByIdAsync(Guid id)
    {
      return await _context.HorariosNegocios.FindAsync(id);
    }

    public async Task AddAsync(HorarioNegocio horarioNegocio)
    {
      await _context.HorariosNegocios.AddAsync(horarioNegocio);
      await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(HorarioNegocio horarioNegocio)
    {
      var existingEntity = await _context.HorariosNegocios.FindAsync(horarioNegocio.IdHorario);
      if (existingEntity != null)
      {
        _context.Entry(existingEntity).State = EntityState.Detached;
      }
      _context.HorariosNegocios.Update(horarioNegocio);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(HorarioNegocio horarioNegocio)
    {
      _context.HorariosNegocios.Remove(horarioNegocio);
      await _context.SaveChangesAsync();
    }

    public async Task<bool> HorarioExistsForDiaAsync(Guid idNegocio, int dia)
    {
      return await _context.HorariosNegocios.AnyAsync(h => h.IdNegocio == idNegocio && h.Dia == dia);
    }
  }
}
