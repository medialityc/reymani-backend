using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class Municipality : BaseEntity
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int ProvinceId { get; set; }
    public Province? Province { get; set; }
  }
}