using System;

namespace reymani_web_api.Endpoints.Auth.Requests
{
  public class ResetPasswordRequest
  {
    // Código de confirmación de 4 dígitos
    public required string ConfirmationCode { get; set; }
    // Nueva contraseña
    public required string Password { get; set; }
  }
}
