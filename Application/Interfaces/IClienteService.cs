using System;
using reymani_web_api.Api.Endpoints.Usuario;

namespace reymani_web_api.Application.Interfaces;

public interface IUsuarioService
{
  Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
  Task<Usuario?> GetUsuarioByIdAsync(Guid id);
  Task UpdateUsuarioAsync(Usuario Usuario);
  Task DeleteUsuarioAsync(Guid id);
  Task AssignRolesToUsuarioAsync(Guid UsuarioId, IEnumerable<Guid> roleIds);
  Task<bool> CheckPasswordAsync(Usuario Usuario, string password);
  Task ChangePasswordAsync(Usuario Usuario, string newPassword);
  Task<List<string>> GetPermissionsAsync(Guid UsuarioId);
  Task ChangeUsuarioStatusAsync(Guid id, bool activo);
}
