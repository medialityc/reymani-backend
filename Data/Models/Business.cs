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
    public required int UserId { get; set; } // Business Administrator
    public User? User { get; set; }
    public required string Address { get; set; }
    public required int MunicipalityId { get; set; }
    public Municipality? Municipality { get; set; }
    public ICollection<Product>? Products { get; set; }
    public required bool IsAvailable { get; set; }
    public required bool IsActive { get; set; }
  }
}