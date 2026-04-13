using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs.CarDtos
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string TransmissionType { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public int SeatingCapacity { get; set; }
        public decimal PricePerDay { get; set; }
        public string Currency { get; set; } = "USD";
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
