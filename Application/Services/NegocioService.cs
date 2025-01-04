namespace reymani_web_api.Application.Services;

public class NegocioService : INegocioService
{
  private readonly INegocioRepository _negocioRepository;

  public NegocioService(INegocioRepository negocioRepository)
  {
    _negocioRepository = negocioRepository;
  }

  public async Task<Negocio> AddAsync(Negocio negocio)
  {
    return await _negocioRepository.AddAsync(negocio);
  }

  public async Task<Negocio?> GetByIdAsync(Guid id)
  {
    return await _negocioRepository.GetByIdAsync(id);
  }

  public async Task<IEnumerable<Negocio>> GetAllAsync()
  {
    return await _negocioRepository.GetAllAsync();
  }

  public async Task UpdateAsync(Negocio negocio)
  {
    await _negocioRepository.UpdateAsync(negocio);
  }

  public async Task DeleteAsync(Guid id)
  {
    await _negocioRepository.DeleteAsync(id);
  }
}
