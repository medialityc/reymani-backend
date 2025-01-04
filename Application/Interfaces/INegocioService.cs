namespace reymani_web_api.Application.Interfaces;

public interface INegocioService
{
  Task<Negocio> AddAsync(Negocio negocio);
  Task<Negocio?> GetByIdAsync(Guid id);
  Task<IEnumerable<Negocio>> GetAllAsync();
  Task UpdateAsync(Negocio negocio);
  Task DeleteAsync(Guid id);
}
