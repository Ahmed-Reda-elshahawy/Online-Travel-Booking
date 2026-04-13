using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.DeleteFlight;

public record DeleteFlightCommand(int Id) : IRequest<Result>;
