namespace reymani_web_api.Application.Services;

public class NegocioService : INegocioService
{
  private readonly INegocioRepository _negocioRepository;
  private readonly ICategoriaNegocioRepository _categoriaNegocioRepository;

  public NegocioService(INegocioRepository negocioRepository, ICategoriaNegocioRepository categoriaNegocioRepository)
  {
    _negocioRepository = negocioRepository;
    _categoriaNegocioRepository = categoriaNegocioRepository;
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

  public async Task AssignCategoriasNegocioToNegocioAsync(Guid negocioId, List<Guid> categoriaNegocioIds)
  {
    var negocio = await _negocioRepository.GetByIdAsync(negocioId);
    if (negocio == null)
    {
      throw new Exception("Negocio no encontrado.");
    }

    negocio.NegocioCategorias.Clear();
    foreach (var categoriaNegocioId in categoriaNegocioIds)
    {
      var categoriaNegocio = await _categoriaNegocioRepository.GetByIdAsync(categoriaNegocioId);
      if (categoriaNegocio == null)
      {
        throw new Exception($"Categoría de Negocio con ID {categoriaNegocioId} no encontrada.");
      }
      negocio.NegocioCategorias.Add(new NegocioCategoria
      {
        IdNegocio = negocioId,
        IdCategoria = categoriaNegocioId,
        Negocio = negocio,
        Categoria = categoriaNegocio
      });
    }

    await _negocioRepository.UpdateAsync(negocio);
  }
}
