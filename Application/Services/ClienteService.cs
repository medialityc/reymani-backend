using reymani_web_api.Application.Utils;

namespace reymani_web_api.Application.Services;

public class UsuarioService : IUsuarioService
{
  private readonly IUsuarioRepository _UsuarioRepository;
  private readonly IRolRepository _rolRepository;

  public UsuarioService(IUsuarioRepository UsuarioRepository, IRolRepository rolRepository)
  {
    _UsuarioRepository = UsuarioRepository;
    _rolRepository = rolRepository;
  }

  public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
  {
    return await _UsuarioRepository.GetAllAsync();
  }

  public async Task<Usuario?> GetUsuarioByIdAsync(Guid id)
  {
    return await _UsuarioRepository.GetByIdAsync(id);
  }

  public async Task UpdateUsuarioAsync(Usuario Usuario)
  {
    await _UsuarioRepository.UpdateAsync(Usuario);
  }

  public async Task DeleteUsuarioAsync(Guid id)
  {
    var Usuario = await _UsuarioRepository.GetByIdAsync(id);

    if (Usuario == null)
    {
      throw new Exception("Usuario no encontrado.");
    }

    await _UsuarioRepository.DeleteAsync(Usuario);

  }

  public Task<bool> CheckPasswordAsync(Usuario Usuario, string password)
  {
    return Task.FromResult(HashPassword.VerifyHash(password, Usuario.PasswordHash));
  }

  public Task ChangePasswordAsync(Usuario Usuario, string newPassword)
  {
    Usuario.PasswordHash = HashPassword.ComputeHash(newPassword);
    return _UsuarioRepository.UpdateAsync(Usuario);
  }

  public async Task AssignRolesToUsuarioAsync(Guid UsuarioId, IEnumerable<Guid> roleIds)
  {
    var Usuario = await _UsuarioRepository.GetByIdAsync(UsuarioId);
    if (Usuario == null)
    {
      throw new Exception("Usuario no encontrado.");
    }

    Usuario.Roles.Clear();
    foreach (var roleId in roleIds)
    {
      var rol = await _rolRepository.GetByIdAsync(roleId);
      if (rol == null)
      {
        throw new Exception($"Rol con ID {roleId} no encontrado.");
      }
      Usuario.Roles.Add(new UsuarioRol { IdUsuario = UsuarioId, IdRol = roleId });
    }

    await _UsuarioRepository.UpdateAsync(Usuario);
  }

  public async Task<List<string>> GetPermissionsAsync(Guid UsuarioId)
  {
    var permisos = await _UsuarioRepository.GetPermissionsAsync(UsuarioId);

    return permisos;
  }

  public async Task ChangeUsuarioStatusAsync(Guid id, bool activo)
  {
    var Usuario = await _UsuarioRepository.GetByIdAsync(id);
    if (Usuario == null)
    {
      throw new Exception("Usuario no encontrado.");
    }

    Usuario.Activo = activo;
    await _UsuarioRepository.UpdateAsync(Usuario);
  }
}
