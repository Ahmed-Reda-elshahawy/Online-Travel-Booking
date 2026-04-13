using MediatR;
using OnlineTravelBooking.Application.DTOs.TourDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.TourFeature.Commands
{
    public class CancelBookingTour : IRequest
    {
        public TourBookingDto BookingDto { get; set; }
        public BookingDto BaseBookingDto { get; set; }
        public CancelBookingTour(TourBookingDto bookingDto, BookingDto baseBookingDto)
        {
            BookingDto = bookingDto;
            BaseBookingDto = baseBookingDto;
        }
    }
}
