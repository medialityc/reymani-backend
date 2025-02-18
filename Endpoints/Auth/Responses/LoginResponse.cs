using System;

namespace reymani_web_api.Endpoints.Auth.Responses;

public class LoginResponse
{
  public required string Token { get; set; }
}