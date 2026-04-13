using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetAllFlights;

public record GetAllFlightsQuery() : IRequest<Result<List<FlightDto>>>;
