using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class Municipality : BaseEntity
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public int ProvinceId { get; set; }
    public required Province Province { get; set; }
  }
}