using OnlineTravelBooking.Domain.Entities.Tour;

namespace OnlineTravelBooking.MVC.Models
{
    public class TourScheduleDto
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailableSeats { get; set; }
    }
}
