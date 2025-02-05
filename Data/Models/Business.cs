using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class Business : BaseEntity
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required string? Logo { get; set; }
    public required string? Banner { get; set; }
    public int UserId { get; set; } // Business Administrator
    public required User User { get; set; }
    public required string Address { get; set; }
    public int MunicipalityId { get; set; }
    public required Municipality Municipality { get; set; }
    public required ICollection<Product> Products { get; set; }
    public required bool IsAvailable { get; set; }
    public required bool IsActive { get; set; }
  }
}