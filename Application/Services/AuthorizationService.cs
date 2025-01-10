namespace reymani_web_api.Application.Services;

public class AuthorizationService : IAuthorizationService
{
  private readonly IRolRepository _rolRepository;

  public AuthorizationService(IRolRepository rolRepository)
  {
    _rolRepository = rolRepository;
  }

  public async Task<bool> IsRoleAuthorizedToEndpointAsync(Guid[] IdRoles, string CodigoPermiso)
  {
    foreach (var roleId in IdRoles)
    {
      var permisos = await _rolRepository.GetCodigosPermisosRolAsync(roleId);
      if (permisos.Contains(CodigoPermiso))
      {
        return true;
      }
    }
    return false;
  }
}
