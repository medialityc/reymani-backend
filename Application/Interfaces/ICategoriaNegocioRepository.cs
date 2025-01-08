using System;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Interfaces
{
  public interface ICategoriaNegocioRepository
  {
    Task<IEnumerable<CategoriaNegocio>> GetAllAsync();
    Task<CategoriaNegocio?> GetByIdAsync(Guid id);
    Task<CategoriaNegocio?> GetByNameAsync(string nombre);
    Task AddAsync(CategoriaNegocio categoriaNegocio);
    Task UpdateAsync(CategoriaNegocio categoriaNegocio);
    Task DeleteAsync(CategoriaNegocio categoriaNegocio);
  }
}
