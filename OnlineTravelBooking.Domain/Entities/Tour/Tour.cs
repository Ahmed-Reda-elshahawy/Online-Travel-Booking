using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTravelBooking.Domain.Entities.Tour
{
    public class Tour : BaseEntity
    {
        public string Title { get; set; }= null!;
        public string Slug { get; set; }= null!;
        public string Description { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string duration { get; set; } = null!;
        public Point MeetingPoint { get; set; }  = null!;
        public string ProviderName { get; set; } = null!;
        public string ProviderContact { get; set; } = null!;
        public int ViewCount { get; set; }
        public int BookingCount { get; set; }
        public decimal AverageRating { get; set; }
        public decimal Price { get; set; }
        public int ReviewCount { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public TourCategory Category { get; set; } = null!;
        public ICollection<TourImage> Images { get; set; } = new List<TourImage>();
        public ICollection<TourSchedule> Schedules { get; set; } = new List<TourSchedule>();
    }
}
