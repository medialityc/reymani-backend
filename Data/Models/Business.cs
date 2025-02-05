using System;
using System.Collections.Generic;

namespace ReymaniWebApi.Data.Models
{
    public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public string? Banner { get; set; }
        public int UserId { get; set; } // Business Administrator
        public User User { get; set; }
        public string Address { get; set; }
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }
        public ICollection<Product> Products { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}