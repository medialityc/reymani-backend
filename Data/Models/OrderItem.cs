using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class OrderItem : BaseEntity
  {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public required Order Order { get; set; }
    public int ProductId { get; set; }
    public required Product Product { get; set; }
    public required int Quantity { get; set; }
  }
}