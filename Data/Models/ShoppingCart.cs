using System;
using System.Collections.Generic;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class ShoppingCart : BaseEntity
  {
    public int Id { get; set; }
    public ICollection<ShoppingCartItem>? Items { get; set; }
    public required int UserId { get; set; } // Customer User
    public User? Customer { get; set; }
    public required int UserAddressId { get; set; }
    public UserAddress? UserAddress { get; set; }
  }
}