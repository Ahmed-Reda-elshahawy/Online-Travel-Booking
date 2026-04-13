using OnlineTravelBooking.Domain.Entities.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs.TourDtos
{
    public class TourBookingDto
    {
        public int Id { get; set; }
        public int TourScheduleId { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal unitPrice { get; set; }
        public int BookingId { get; set; }
    }
}
