using NetTopologySuite.Geometries;
using OnlineTravelBooking.Domain.Entities.Tour;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTravelBooking.MVC.Models
{
    public class TourDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public IFormFile? Image { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public string duration { get; set; } = null!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ProviderName { get; set; } = null!;
        public string ProviderContact { get; set; } = null!;
        public int ViewCount { get; set; } = 0;
        public int BookingCount { get; set; } = 0;
        public decimal AverageRating { get; set; } = 0;
        public decimal Price { get; set; }
        public int ReviewCount { get; set; } = 0;
        
        public int CategoryId { get; set; }
    }
}
