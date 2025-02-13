using FastEndpoints;

using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Endpoints.Users.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Utils.Mappers;

public class UserMapper
{
  public User ToEntity(CreateUserRequest r) => new()
  {
    ProfilePicture = string.Empty,
    FirstName = r.FirstName,
    LastName = r.LastName,
    Email = r.Email,
    Phone = r.Phone,
    Password = r.Password,
    IsActive = r.IsActive,
    Role = r.Role,
    IsConfirmed = r.IsConfirmed
  };

  public UserResponse FromEntity(User e) => new()
  {
    Id = e.Id,
    ProfilePicture = e.ProfilePicture,
    FirstName = e.FirstName,
    LastName = e.LastName,
    Email = e.Email,
    Phone = e.Phone,
    IsActive = e.IsActive,
    Role = e.Role,
    IsConfirmed = e.IsConfirmed
  };

  // Método para actualizar un usuario en el endpoint de update user
  public User ToEntity(UpdateUserRequest r, User existing)
  {
    existing.FirstName = r.FirstName;
    existing.LastName = r.LastName;
    existing.Email = r.Email;
    existing.Phone = r.Phone;
    // Si la contraseña se envía, se actualiza (con hash)
    if (!string.IsNullOrEmpty(r.Password))
      existing.Password = BCrypt.Net.BCrypt.HashPassword(r.Password);
    existing.IsActive = r.IsActive;
    existing.Role = r.Role;
    existing.IsConfirmed = r.IsConfirmed;
    return existing;
  }

  // Método para actualizar un usuario en el endpoint de update me
  public User ToEntity(UpdateMeRequest r, User existing)
  {
    existing.FirstName = r.FirstName;
    existing.LastName = r.LastName;
    existing.Email = r.Email;
    existing.Phone = r.Phone;
    return existing;
  }
}
