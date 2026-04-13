using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetFlightById;

public record GetFlightByIdQuery(int FlightId) : IRequest<Result<FlightDetailsDto>>;
