using System;

namespace reymani_web_api.Application.Interfaces;

public interface IAuthorizationService
{
  public Task<bool> IsRoleAuthorizedToEndpointAsync(Guid[] IdRoles, string CodigoPermiso);
}
