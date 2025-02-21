using System;

using Microsoft.AspNetCore.Http;

using reymani_web_api.Data.Models;

namespace reymani_web_api.Endpoints.Users.Requests
{
  public class CreateUserRequest
  {
    public IFormFile? ProfilePicture { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required bool IsActive { get; set; }
    public required UserRole Role { get; set; }
    public required bool IsConfirmed { get; set; }
  }
}