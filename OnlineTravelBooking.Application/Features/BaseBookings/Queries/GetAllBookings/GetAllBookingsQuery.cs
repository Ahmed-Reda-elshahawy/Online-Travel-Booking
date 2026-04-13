using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.BaseBookingDtos;

namespace OnlineTravelBooking.Application.Features.BaseBookings.Queries.GetAllBookings;

public record GetAllBookingsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<BaseBookingDto>>;