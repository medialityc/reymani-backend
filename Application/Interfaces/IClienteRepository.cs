using System;

namespace reymani_web_api.Application.Interfaces;

public interface IUsuarioRepository
{
  Task<IEnumerable<Usuario>> GetAllAsync();
  Task<Usuario?> GetByIdAsync(Guid id);
  Task AddAsync(Usuario Usuario);
  Task UpdateAsync(Usuario Usuario);
  Task DeleteAsync(Usuario Usuario);
  Task<Usuario?> GetUsuarioByUsernameOrPhoneAsync(string usernameOrPhone);
  Task<string[]> GetIdRolesUsuarioAsync(Guid id);
  Task<List<string>> GetPermissionsAsync(Guid UsuarioId);
}