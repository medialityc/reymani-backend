using Microsoft.EntityFrameworkCore;

namespace reymani_web_api.Infraestructure.Repositories
{
  public class NegocioClienteRepository : INegocioClienteRepository
  {
    private readonly DBContext _context;

    public NegocioClienteRepository(DBContext context)
    {
      _context = context;
    }

    public async Task AddAsync(NegocioCliente negocioCliente)
    {
      await _context.NegociosClientes.AddAsync(negocioCliente);
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(NegocioCliente negocioCliente)
    {
      _context.NegociosClientes.Remove(negocioCliente);
      await _context.SaveChangesAsync();
    }

    public async Task<NegocioCliente?> GetByIdAsync(Guid id)
    {
      return await _context.NegociosClientes.FindAsync(id);
    }

    public async Task<IEnumerable<NegocioCliente>> GetAllAsync()
    {
      return await _context.NegociosClientes.ToListAsync();
    }

    public async Task<IEnumerable<Negocio>> GetNegociosByClienteIdAsync(Guid clienteId)
    {
      return await _context.NegociosClientes
          .Where(nc => nc.IdCliente == clienteId)
          .Select(nc => nc.Negocio!)
          .ToListAsync();
    }

    public async Task<IEnumerable<Cliente>> GetClientesByNegocioIdAsync(Guid negocioId)
    {
      return await _context.NegociosClientes
          .Where(nc => nc.IdNegocio == negocioId)
          .Select(nc => nc.Cliente!)
          .ToListAsync();
    }
  }
}
