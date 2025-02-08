using System;

namespace reymani_web_api.Endpoints.Auth.Requests;

public class ForgotPasswordRequest
{
  public required string Email { get; set; }
}
