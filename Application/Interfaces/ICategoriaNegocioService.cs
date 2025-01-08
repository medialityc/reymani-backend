using System;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Interfaces
{
  public interface ICategoriaNegocioService
  {
    Task<IEnumerable<CategoriaNegocio>> GetAllAsync();
    Task<CategoriaNegocio?> GetByIdAsync(Guid id);
    Task AddAsync(CategoriaNegocio categoriaNegocio);
    Task UpdateAsync(CategoriaNegocio categoriaNegocio);
    Task DeleteAsync(Guid id);
  }
}
