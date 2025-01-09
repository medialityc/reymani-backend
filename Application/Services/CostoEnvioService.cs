using System;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Services
{
  public class CostoEnvioService : ICostoEnvioService
  {
    private readonly ICostoEnvioRepository _costoEnvioRepository;

    public CostoEnvioService(ICostoEnvioRepository costoEnvioRepository)
    {
      _costoEnvioRepository = costoEnvioRepository;
    }

    public async Task<IEnumerable<CostoEnvio>> GetAllAsync()
    {
      return await _costoEnvioRepository.GetAllAsync();
    }

    public async Task<CostoEnvio?> GetByIdAsync(Guid id)
    {
      return await _costoEnvioRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(CostoEnvio costoEnvio)
    {
      await _costoEnvioRepository.AddAsync(costoEnvio);
    }

    public async Task UpdateAsync(CostoEnvio costoEnvio)
    {
      await _costoEnvioRepository.UpdateAsync(costoEnvio);
    }

    public async Task DeleteAsync(Guid id)
    {
      var costoEnvio = await GetByIdAsync(id);
      if (costoEnvio == null)
      {
        throw new Exception("Costo de envío no encontrado.");
      }
      await _costoEnvioRepository.DeleteAsync(costoEnvio);
    }

    public async Task<CostoEnvio?> GetByNegocioAndDistanciaMaxKmAsync(Guid idNegocio, int distanciaMaxKm)
    {
      return await _costoEnvioRepository.GetByNegocioAndDistanciaMaxKmAsync(idNegocio, distanciaMaxKm);
    }
  }
}
