using Microsoft.VisualBasic.FileIO;
using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Cars
{
    public enum FuelType
    {
        Petrol = 1,
        Diesel = 2,
        Electric = 3,
        Hybrid = 4
    }

    public enum TransmissionType
    {
        Manual=1,
        Automatic=2,
        SemiAutomatic=3
    }

    public enum CarStatus
    {
        Available=1,
        Rented=2,
        Maintenance=3,
        Retired=4
    }
    public class Car: BaseEntity
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
        public int? CategoryId { get; set; }
        public string? LicensePlate { get; set; }
        public string? Color { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<BaseBooking> Bookings { get; set; } = new List<BaseBooking>();
        public CarCategory Category { get; set; }
        public ICollection<CarAvailability> Availabilities { get; set; }
        public ICollection<CarImage> Images { get; set; }
        public ICollection<CarPricingTier> PricingTiers { get; set; }
        public ICollection<CarExtra> Extras { get; set; }
    }
}
