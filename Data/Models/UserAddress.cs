using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class UserAddress : BaseEntity
  {
    public int Id { get; set; }
    public int UserId { get; set; } // Customer User
    public required User User { get; set; }
    public required string Name { get; set; }
    public required string? Notes { get; set; }
    public required string Address { get; set; }
    public required int MunicipalityId { get; set; }
    public Municipality? Municipality { get; set; }
    public required bool IsActive { get; set; }
  }
}