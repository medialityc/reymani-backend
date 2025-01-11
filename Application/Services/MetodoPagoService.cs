namespace reymani_web_api.Application.Services;

public class MetodoPagoService : IMetodoPagoService
{
  private readonly IMetodoPagoRepository _metodoPagoRepository;

  public MetodoPagoService(IMetodoPagoRepository metodoPagoRepository)
  {
    _metodoPagoRepository = metodoPagoRepository;
  }

  public async Task<MetodoPago> AddAsync(MetodoPago metodoPago)
  {
    return await _metodoPagoRepository.AddAsync(metodoPago);
  }

  public async Task<MetodoPago?> GetByIdAsync(Guid id)
  {
    return await _metodoPagoRepository.GetByIdAsync(id);
  }

  public async Task<IEnumerable<MetodoPago>> GetAllAsync()
  {
    return await _metodoPagoRepository.GetAllAsync();
  }

  public async Task UpdateAsync(MetodoPago metodoPago)
  {
    await _metodoPagoRepository.UpdateAsync(metodoPago);
  }

  public async Task DeleteAsync(Guid id)
  {
    await _metodoPagoRepository.DeleteAsync(id);
  }

  public async Task<IEnumerable<MetodoPago>> GetAllByIdEntidadAsync(Guid idEntidad)
  {
    return await _metodoPagoRepository.GetAllByIdEntidadAsync(idEntidad);
  }

  public async Task<bool> ExistsByEntidadAndProveedorAsync(Guid idEntidad, string proveedor)
  {
    return await _metodoPagoRepository.ExistsByEntidadAndProveedorAsync(idEntidad, proveedor);
  }
}
