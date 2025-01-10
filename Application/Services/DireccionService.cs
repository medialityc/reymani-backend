
namespace reymani_web_api.Application.Services
{
  public class DireccionService : IDireccionService
  {
    private readonly IDireccionRepository _direccionRepository;

    public DireccionService(IDireccionRepository direccionRepository)
    {
      _direccionRepository = direccionRepository;
    }

    public async Task<Direccion> AddAsync(Direccion direccion)
    {
      return await _direccionRepository.AddAsync(direccion);
    }

    public async Task<Direccion?> GetByIdAsync(Guid id)
    {
      return await _direccionRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Direccion>> GetAllAsync()
    {
      return await _direccionRepository.GetAllAsync();
    }

    public async Task UpdateAsync(Direccion direccion)
    {
      await _direccionRepository.UpdateAsync(direccion);
    }

    public async Task DeleteAsync(Guid id)
    {
      await _direccionRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Direccion>> GetAllByIdEntidadAsync(Guid idEntidad)
    {
      return await _direccionRepository.GetAllByIdEntidadAsync(idEntidad);
    }
  }
}
