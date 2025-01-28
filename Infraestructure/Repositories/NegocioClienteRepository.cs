using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Infraestructure.Repositories
{
  public class NegocioUsuarioRepository : INegocioUsuarioRepository
  {
    private readonly DBContext _context;

    public NegocioUsuarioRepository(DBContext context)
    {
      _context = context;
    }

    public async Task AddAsync(NegocioUsuario negocioUsuario)
    {
      await _context.NegociosUsuarios.AddAsync(negocioUsuario);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid UsuarioId, Guid negocioId)
    {
      var negocioUsuario = await _context.NegociosUsuarios
        .FirstOrDefaultAsync(nc => nc.IdUsuario == UsuarioId && nc.IdNegocio == negocioId);
      if (negocioUsuario != null)
      {
        _context.NegociosUsuarios.Remove(negocioUsuario);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<NegocioUsuario?> GetByIdAsync(Guid id)
    {
      return await _context.NegociosUsuarios.FindAsync(id);
    }

    public async Task<IEnumerable<NegocioUsuario>> GetAllAsync()
    {
      return await _context.NegociosUsuarios.ToListAsync();
    }

    public async Task<IEnumerable<Negocio>> GetNegociosByUsuarioIdAsync(Guid UsuarioId)
    {
      return await _context.NegociosUsuarios
          .Where(nc => nc.IdUsuario == UsuarioId)
          .Select(nc => nc.Negocio!)
          .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetUsuariosByNegocioIdAsync(Guid negocioId)
    {
      return await _context.NegociosUsuarios
          .Where(nc => nc.IdNegocio == negocioId)
          .Select(nc => nc.Usuario!)
          .ToListAsync();
    }

    public async Task<NegocioUsuario?> GetByIdUsuarioAndIdNegocio(Guid idUsuario, Guid idNegocio)
    {
      return await _context.NegociosUsuarios
        .FirstOrDefaultAsync(nc => nc.IdUsuario == idUsuario && nc.IdNegocio == idNegocio);
    }
  }
}
