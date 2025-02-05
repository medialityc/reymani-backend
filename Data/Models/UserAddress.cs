using System;

namespace ReymaniWebApi.Data.Models
{
    public class UserAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Customer User
        public User User { get; set; }
        public string Name { get; set; }
        public string? Notes { get; set; }
        public string Address { get; set; }
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}