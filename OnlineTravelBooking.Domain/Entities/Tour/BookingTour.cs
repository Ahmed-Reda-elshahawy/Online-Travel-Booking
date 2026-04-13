using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.Domain.Entities.Tour
{
    public class BookingTour : BaseEntity
    {
        public int TourScheduleId { get; set; }
        public TourSchedule TourSchedule { get; set; } = null!;
        public int NumberOfGuests { get; set; }
        public decimal unitPrice { get; set; }
        public int BookingId { get; set; }
        public BaseBooking Booking { get; set; } = null!;
    }
}
