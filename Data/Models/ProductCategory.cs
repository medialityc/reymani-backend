using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class ProductCategory : BaseEntity
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string? Logo { get; set; }
    public required bool IsActive { get; set; }
  }
}