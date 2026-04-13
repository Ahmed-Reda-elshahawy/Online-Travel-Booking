using AutoMapper;
using MediatR;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.BaseBookingDtos;

namespace OnlineTravelBooking.Application.Features.BaseBookings.Queries.GetAllBookings;

public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, PaginatedList<BaseBookingDto>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetAllBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BaseBookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var paginatedBookings = await _bookingRepository.GetAllBookingsAsync(request.PageNumber, request.PageSize);

        var bookingDtos = _mapper.Map<List<BaseBookingDto>>(paginatedBookings.Items);

        return new PaginatedList<BaseBookingDto>(
            bookingDtos,
            paginatedBookings.TotalCount,
            paginatedBookings.PageNumber,
            paginatedBookings.PageSize
        );
    }
}
