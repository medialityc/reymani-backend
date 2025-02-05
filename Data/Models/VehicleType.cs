using System;

namespace ReymaniWebApi.Data.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
        public int TotalCapacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}