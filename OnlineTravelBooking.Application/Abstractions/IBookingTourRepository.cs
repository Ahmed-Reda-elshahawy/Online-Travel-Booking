using OnlineTravelBooking.Domain.Entities.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Abstractions
{
    public interface IBookingTourRepository
    {
        public Task<BookingTour> GetBookingTourDetailsAsync(int bookingTourId);
        public Task AddBookingTourAsync(BookingTour bookingTour);
        public void UpdateBookingTourAsync(BookingTour bookingTour);
    }
}
