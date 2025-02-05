using System;

namespace ReymaniWebApi.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Courier
        public User User { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public string? Picture { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}