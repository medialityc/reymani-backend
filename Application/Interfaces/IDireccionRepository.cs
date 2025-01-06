using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Interfaces
{
  public interface IDireccionRepository
  {
    Task<Direccion> AddAsync(Direccion direccion);
    Task<Direccion?> GetByIdAsync(Guid id);
    Task<IEnumerable<Direccion>> GetAllAsync();
    Task UpdateAsync(Direccion direccion);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Direccion>> GetAllByIdEntidadAsync(Guid idEntidad);
  }
}
