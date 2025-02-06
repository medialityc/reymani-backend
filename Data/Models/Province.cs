using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class Province : BaseEntity
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Municipality>? Municipalities { get; set; }
  }
}