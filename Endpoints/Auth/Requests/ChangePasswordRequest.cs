using System;

namespace reymani_web_api.Endpoints.Auth.Requests;

public class ChangePasswordRequest
{
  public required string CurrentPassword { get; set; }
  public required string NewPassword { get; set; }
}