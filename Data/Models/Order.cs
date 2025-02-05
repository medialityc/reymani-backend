using System;
using System.Collections.Generic;
using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int CustomerUserId { get; set; }
        public User Customer { get; set; }
        public int? CourierUserId { get; set; }
        public User Courier { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public OrderStatus Status { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalProductsCost { get; set; }
        public int UserAddressId { get; set; }
        public UserAddress UserAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}