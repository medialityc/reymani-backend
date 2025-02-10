using System;

namespace reymani_web_api.Endpoints.Auth.Requests;

public class ConfirmEndpointRequest
{
  public required string ConfirmationCode { get; set; }
  public required string Email { get; set; }
}
