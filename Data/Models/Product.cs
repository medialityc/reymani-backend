using System;
using System.Collections.Generic;

namespace ReymaniWebApi.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public List<string>? Images { get; set; } // Optional
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public double Rating { get; set; } // 0 to 5
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }
        public int Capacity { get; set; } // High -> 3, Medium -> 2, Low -> 1
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}