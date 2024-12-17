namespace reymani_web_api.Application.Services
{
  public class PermisoService : IPermisoService
  {
    private readonly IPermisoRepository _permisoRepository;

    public PermisoService(IPermisoRepository permisoRepository)
    {
      _permisoRepository = permisoRepository;
    }

    public async Task<IEnumerable<Permiso>> GetAllAsync()
    {
      return await _permisoRepository.GetAllAsync();
    }

    public async Task<Permiso?> GetByIdAsync(Guid id)
    {
      return await _permisoRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Permiso permiso)
    {
      await _permisoRepository.AddAsync(permiso);
    }

    public async Task UpdateAsync(Permiso permiso)
    {
      await _permisoRepository.UpdateAsync(permiso);
    }

    public async Task DeleteAsync(Guid id)
    {
      var permiso = await GetByIdAsync(id);
      if (permiso == null)
      {
        throw new Exception("Permiso no encontrado.");
      }
      await _permisoRepository.DeleteAsync(permiso);
    }
  }
}
