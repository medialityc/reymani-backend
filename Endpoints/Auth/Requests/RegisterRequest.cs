using System;

namespace reymani_web_api.Endpoints.Auth.Requests;

public class RegisterRequest
{
  public required string Email { get; set; }
  public required string Password { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Phone { get; set; }
  public required IFormFile? ProfilePicture { get; set; }

}


