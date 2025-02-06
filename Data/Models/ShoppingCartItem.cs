using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class ShoppingCartItem : BaseEntity
  {
    public int Id { get; set; }
    public required int ShoppingCartId { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public required int Quantity { get; set; }
  }
}