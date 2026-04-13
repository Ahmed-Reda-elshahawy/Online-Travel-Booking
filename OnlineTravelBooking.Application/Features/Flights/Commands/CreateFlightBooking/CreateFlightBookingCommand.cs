using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.CreateFlightBooking;

public record CreateFlightBookingCommand(CreateFlightBookingRequest Request) : IRequest<Result<CreateFlightBookingResponse>>;