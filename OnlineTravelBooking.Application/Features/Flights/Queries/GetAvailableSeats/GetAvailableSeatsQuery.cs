using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetAvailableSeats;

public record GetAvailableSeatsQuery(int FlightId) : IRequest<Result<AvailableSeatsDto>>;