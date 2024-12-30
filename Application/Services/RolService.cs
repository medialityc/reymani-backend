using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Application.Services
{
  public class RolService : IRolService
  {
    private readonly IRolRepository _rolRepository;

    public RolService(IRolRepository rolRepository)
    {
      _rolRepository = rolRepository;
    }

    public async Task<IEnumerable<Rol>> GetAllAsync()
    {
      return await _rolRepository.GetAllAsync();
    }

    public async Task<Rol?> GetByIdAsync(Guid id)
    {
      return await _rolRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Rol rol, IEnumerable<Guid> permisoIds)
    {
      await _rolRepository.AddAsync(rol, permisoIds);
    }

    public async Task UpdateAsync(Rol rol)
    {
      await _rolRepository.UpdateAsync(rol);
    }

    public async Task DeleteAsync(Guid id)
    {
      var rol = await GetByIdAsync(id);
      if (rol == null)
      {
        throw new Exception("Rol no encontrado.");
      }
      await _rolRepository.DeleteAsync(rol);
    }

    public async Task AssignPermissionsAsync(Guid rolId, IEnumerable<Guid> permisoIds)
    {
      await _rolRepository.AssignPermissionsAsync(rolId, permisoIds);
    }

    public async Task<bool> RolNameExistsAsync(string nombre)
    {
      return await _rolRepository.RolNameExistsAsync(nombre);
    }
  }
}
