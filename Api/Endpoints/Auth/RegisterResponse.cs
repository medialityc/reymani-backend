using System;
using System.Collections.Generic;

namespace reymani_web_api.Api.Endpoints.Auth
{
  public class RegisterResponse
  {
    public required string Token { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
  }
}
