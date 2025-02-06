using System;

namespace reymani_web_api.Endpoints.Auth.Requests;

public class LoginRequest
{
  public required string Email { get; set; }
  public required string Password { get; set; }
}
