using System;

namespace reymani_web_api.Endpoints.Auth.Requests
{
  public class ResetPasswordRequest
  {
    public required string ConfirmationCode { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
  }
}