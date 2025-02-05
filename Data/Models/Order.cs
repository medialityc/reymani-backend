using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class Order : BaseEntity
  {
    public int Id { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public int CustomerUserId { get; set; }
    public required User Customer { get; set; }
    public int? CourierUserId { get; set; }
    public required User? Courier { get; set; }
    public required ICollection<OrderItem> Items { get; set; }
    public required OrderStatus Status { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalProductsCost { get; set; }
    public int UserAddressId { get; set; }
    public required UserAddress UserAddress { get; set; }
  }
}