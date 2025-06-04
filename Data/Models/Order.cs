using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class Order : BaseEntity
  {
    public int Id { get; set; }
    public required PaymentMethod PaymentMethod { get; set; }
    public required int CustomerId { get; set; }
    public User? Customer { get; set; }
    public required bool RequiresCourierService { get; set; }
    public int? CourierId { get; set; }
    public User? Courier { get; set; }
    public ICollection<OrderItem>? Items { get; set; }
    public required OrderStatus Status { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalProductsCost { get; set; }
    public required int CustomerAddressId { get; set; }
    public UserAddress? CustomerAddress { get; set; }
  }
}