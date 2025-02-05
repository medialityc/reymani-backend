using System;
using System.Collections.Generic;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class ShoppingCart : BaseEntity
  {
    public int Id { get; set; }
    public required ICollection<ShoppingCartItem> Items { get; set; }
    public int UserId { get; set; } // Customer User
    public required User Customer { get; set; }
    public int UserAddressId { get; set; }
    public required UserAddress UserAddress { get; set; }
  }
}