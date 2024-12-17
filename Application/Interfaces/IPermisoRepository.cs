
namespace reymani_web_api.Application.Interfaces
{
  public interface IPermisoRepository
  {
    Task<IEnumerable<Permiso>> GetAllAsync();
    Task<Permiso?> GetByIdAsync(Guid id);
    Task AddAsync(Permiso permiso);
    Task UpdateAsync(Permiso permiso);
    Task DeleteAsync(Permiso permiso);
  }
}
