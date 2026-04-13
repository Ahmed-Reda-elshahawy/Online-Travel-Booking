using MediatR;
using OnlineTravelBooking.Application.DTOs.TourDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.TourFeature.Queries
{
    public class GetBookingTour : IRequest<BookingContDto>
    {
        public int BookingId { get; set; }
        public GetBookingTour(int bookingId)
        {
            BookingId = bookingId;
        }
    }
}
