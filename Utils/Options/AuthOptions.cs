using System;

namespace reymani_web_api.Utils.Options;

public class AuthOptions
{
  // JWT token secret key
  public required string JwtToken { get; set; }
  // Usuarios del seed
  public required string SystemAdminEmail { get; set; }
  public required string SystemAdminPassword { get; set; }
  public required string BusinessAdminEmail { get; set; }
  public required string BusinessAdminPassword { get; set; }
  public required string CourierEmail { get; set; }
  public required string CourierPassword { get; set; }
  public required string CustomerEmail { get; set; }
  public required string CustomerPassword { get; set; }
}
