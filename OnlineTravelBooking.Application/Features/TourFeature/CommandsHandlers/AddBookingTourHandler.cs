using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Features.TourFeature.Commands;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.TourFeature.CommandsHandlers
{
    public class AddBookingTourHandler : IRequestHandler<AddBookingTour>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingTourRepository _tourRepository;
        public AddBookingTourHandler(IBookingRepository bookingRepository, IBookingTourRepository tourRepository)
        {
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
        }
        public async Task Handle(AddBookingTour request, CancellationToken cancellationToken)
        {
            var BookId = await _bookingRepository.AddBookingAsync(new Domain.Entities.Booking.BaseBooking()
            {
                UserId = request.BaseBookingDto.UserId,
                TourId = request.BaseBookingDto.TourId,
                Category = (BookingCategory) request.BaseBookingDto.Category,
                Status = (BookingStatus) request.BaseBookingDto.Status,
                TotalPrice = request.BaseBookingDto.TotalPrice,
                Currency = request.BaseBookingDto.Currency,
                BookingDate = request.BaseBookingDto.BookingDate,
                PaymentStatus = (PaymentStatus) request.BaseBookingDto.PaymentStatus,
                PaymentMethod = request.BaseBookingDto.PaymentMethod,
            });
            await _tourRepository.AddBookingTourAsync(new Domain.Entities.Tour.BookingTour()
            {
                BookingId = BookId,
                NumberOfGuests = request.BookingDto.NumberOfGuests,
                TourScheduleId = request.BookingDto.TourScheduleId,
                unitPrice = request.BookingDto.unitPrice,
            });
        }
    }
}
