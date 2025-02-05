using System;
using System.Collections.Generic;

namespace ReymaniWebApi.Data.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public ICollection<ShoppingCartItem> Items { get; set; }
        public int UserId { get; set; } // Customer User
        public User Customer { get; set; }
        public int UserAddressId { get; set; }
        public UserAddress UserAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}