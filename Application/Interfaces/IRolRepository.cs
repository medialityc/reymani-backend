using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reymani_web_api.Application.Interfaces
{
  public interface IRolRepository
  {
    Task<IEnumerable<Rol>> GetAllAsync();
    Task<Rol?> GetByIdAsync(Guid id);
    Task AddAsync(Rol rol, IEnumerable<Guid> permisoIds);
    Task UpdateAsync(Rol rol, IEnumerable<Guid> permisoIds);
    Task DeleteAsync(Rol rol);
    Task AssignPermissionsAsync(Guid rolId, IEnumerable<Guid> permisoIds);
    Task<string[]> GetCodigosPermisosRolAsync(Guid id);
    Task<bool> RolNameExistsAsync(string nombre);

    Task<Permiso[]> GetPermisosRolAsync(Guid id);
  }
}
