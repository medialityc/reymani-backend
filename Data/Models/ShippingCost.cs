using System;

namespace ReymaniWebApi.Data.Models
{
    public class ShippingCost
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public int MunicipalityId { get; set; }
        public Municipality Municipality { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}