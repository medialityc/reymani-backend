namespace reymani_web_api.Application.Services
{
  public class CategoriaNegocioService : ICategoriaNegocioService
  {
    private readonly ICategoriaNegocioRepository _categoriaNegocioRepository;

    public CategoriaNegocioService(ICategoriaNegocioRepository categoriaNegocioRepository)
    {
      _categoriaNegocioRepository = categoriaNegocioRepository;
    }

    public async Task<IEnumerable<CategoriaNegocio>> GetAllAsync()
    {
      return await _categoriaNegocioRepository.GetAllAsync();
    }

    public async Task<CategoriaNegocio?> GetByIdAsync(Guid id)
    {
      return await _categoriaNegocioRepository.GetByIdAsync(id);
    }

    public async Task<CategoriaNegocio?> GetByNameAsync(string nombre)
    {
      return await _categoriaNegocioRepository.GetByNameAsync(nombre);
    }

    public async Task AddAsync(CategoriaNegocio categoriaNegocio)
    {
      await _categoriaNegocioRepository.AddAsync(categoriaNegocio);
    }

    public async Task UpdateAsync(CategoriaNegocio categoriaNegocio)
    {
      await _categoriaNegocioRepository.UpdateAsync(categoriaNegocio);
    }

    public async Task DeleteAsync(Guid id)
    {
      var categoriaNegocio = await GetByIdAsync(id);
      if (categoriaNegocio == null)
      {
        throw new Exception("Categoría de negocio no encontrada.");
      }
      await _categoriaNegocioRepository.DeleteAsync(categoriaNegocio);
    }

    public async Task<IEnumerable<CategoriaNegocio>> GetAllByIdNegocioAsync(Guid idNegocio)
    {
      return await _categoriaNegocioRepository.GetAllByIdNegocioAsync(idNegocio);
    }
  }
}
