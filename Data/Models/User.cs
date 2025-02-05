using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class User : BaseEntity
  {
    public int Id { get; set; }
    public required string? ProfilePicture { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; }
    public UserRole Role { get; set; }
  }
}