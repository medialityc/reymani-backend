using System;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Interfaces
{
  public interface ICostoEnvioService
  {
    Task<IEnumerable<CostoEnvio>> GetAllAsync();
    Task<CostoEnvio?> GetByIdAsync(Guid id);
    Task AddAsync(CostoEnvio costoEnvio);
    Task UpdateAsync(CostoEnvio costoEnvio);
    Task DeleteAsync(Guid id);
  }
}
