using System;

namespace reymani_web_api.Application.Interfaces;

public interface IPermisoService
{
  Task<IEnumerable<Permiso>> GetAllAsync();
  Task<Permiso?> GetByIdAsync(Guid id);
  Task AddAsync(Permiso permiso);
  Task UpdateAsync(Permiso permiso);
  Task DeleteAsync(Guid id);
}
