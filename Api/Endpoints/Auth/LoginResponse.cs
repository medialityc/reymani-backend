using System;

namespace reymani_web_api.Api.Endpoints.Auth
{
  public class LoginResponse
  {
    public required string Token { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
  }
}
