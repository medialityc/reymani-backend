using System;
using reymani_web_api.Api.Endpoints.Auth;

namespace reymani_web_api.Application.Interfaces;

public interface IAuthService
{
  Task<bool> IsUsernameInUseAsync(string username);
  Task<bool> IsNumeroCarnetInUseAsync(string numeroCarnet);
  Task<string> RegisterAsync(RegisterRequest request);
  Task<string> LoginAsync(LoginRequest request);
  Guid GetIdUsuarioFromToken(string token);
}
