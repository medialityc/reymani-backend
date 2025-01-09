using System;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Interfaces
{
  public interface ICostoEnvioRepository
  {
    Task<IEnumerable<CostoEnvio>> GetAllAsync();
    Task<CostoEnvio?> GetByIdAsync(Guid id);
    Task AddAsync(CostoEnvio costoEnvio);
    Task UpdateAsync(CostoEnvio costoEnvio);
    Task DeleteAsync(CostoEnvio costoEnvio);
    Task<CostoEnvio?> GetByNegocioAndDistanciaMaxKmAsync(Guid idNegocio, int distanciaMaxKm);
  }
}
