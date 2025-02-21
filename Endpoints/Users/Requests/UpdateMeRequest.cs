using System;

namespace reymani_web_api.Endpoints.Users.Requests;

public class UpdateMeRequest
{
  public IFormFile? ProfilePicture { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string Email { get; set; }
  public required string Phone { get; set; }

}