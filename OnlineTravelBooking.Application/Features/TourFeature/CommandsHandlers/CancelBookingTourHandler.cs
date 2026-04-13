using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Features.TourFeature.Commands;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.TourFeature.CommandsHandlers
{
    
    public class CancelBookingTourHandler : IRequestHandler<CancelBookingTour>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingTourRepository _tourRepository;
        public CancelBookingTourHandler(IBookingRepository bookingRepository, IBookingTourRepository tourRepository)
        {
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
        }

        public Task Handle(CancelBookingTour request, CancellationToken cancellationToken)
        {
             _bookingRepository.UpdateBookingAsync(new Domain.Entities.Booking.BaseBooking()
            {
                 Id= request.BaseBookingDto.Id,
                 UserId = request.BaseBookingDto.UserId,
                TourId = request.BaseBookingDto.TourId,
                Category = (BookingCategory)request.BaseBookingDto.Category,
                Status = (BookingStatus)request.BaseBookingDto.Status,
                TotalPrice = request.BaseBookingDto.TotalPrice,
                Currency = request.BaseBookingDto.Currency,
                BookingDate = request.BaseBookingDto.BookingDate,
                PaymentStatus = (PaymentStatus)request.BaseBookingDto.PaymentStatus,
                PaymentMethod = request.BaseBookingDto.PaymentMethod,
                CancellationReason= request.BaseBookingDto.CancellationReason,
                CancelledAt= DateTime.UtcNow
             });
             _tourRepository.UpdateBookingTourAsync(new Domain.Entities.Tour.BookingTour()
            {
                 Id= request.BookingDto.Id,
                 NumberOfGuests = request.BookingDto.NumberOfGuests,
                TourScheduleId = request.BookingDto.TourScheduleId,
                unitPrice = request.BookingDto.unitPrice,
                BookingId=2
            });
            return Task.CompletedTask;
        }
    }
}
