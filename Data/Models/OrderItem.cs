using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class OrderItem : BaseEntity
  {
    public int Id { get; set; }
    public required int OrderId { get; set; }
    public Order? Order { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public required int Quantity { get; set; }
  }
}