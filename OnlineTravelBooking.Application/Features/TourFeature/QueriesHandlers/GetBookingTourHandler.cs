using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.DTOs.TourDtos;
using OnlineTravelBooking.Application.Features.TourFeature.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Features.TourFeature.QueriesHandlers
{
    public class GetBookingTourHandler : IRequestHandler<GetBookingTour, BookingContDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingTourRepository _tourRepository;
        public GetBookingTourHandler(IBookingRepository bookingRepository, IBookingTourRepository tourRepository)
        {
            _bookingRepository = bookingRepository;
            _tourRepository = tourRepository;
        }
        public async Task<BookingContDto> Handle(GetBookingTour request, CancellationToken cancellationToken)
        {
            var Base=await _bookingRepository.GetBookingDetailsAsync(request.BookingId);
            var Tour=await _tourRepository.GetBookingTourDetailsAsync(request.BookingId);
            return new BookingContDto()
            {
                Id= Tour.Id,
                TourScheduleId= Tour.TourScheduleId,
                NumberOfGuests= Tour.NumberOfGuests,
                unitPrice= Tour.unitPrice,
                BookingId= Tour.BookingId,
                TourId= Base.TourId ?? 0,
                Category= (int)Base.Category,
                Status= (int)Base.Status,
                TotalPrice= Base.TotalPrice,
                Currency= Base.Currency,
                BookingDate= Base.BookingDate,
                PaymentStatus= (int)Base.PaymentStatus,
                PaymentMethod= Base.PaymentMethod,
                CancelledAt= Base.CancelledAt,
                CancellationReason= Base.CancellationReason,
                RefundAmount= Base.RefundAmount,
                RefundProcessedAt= Base.RefundProcessedAt
            };
        }
    }
}
