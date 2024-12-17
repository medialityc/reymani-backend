using System;

namespace reymani_web_api.Application.Interfaces;

public interface IRolService
{
  Task<IEnumerable<Rol>> GetAllAsync();
  Task<Rol?> GetByIdAsync(Guid id);
  Task AddAsync(Rol rol);
  Task UpdateAsync(Rol rol);
  Task DeleteAsync(Guid id);
  Task AssignPermissionsAsync(Guid rolId, IEnumerable<Guid> permisoIds);
}
