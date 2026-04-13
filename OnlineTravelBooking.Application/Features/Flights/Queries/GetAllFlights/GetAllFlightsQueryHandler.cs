using MediatR;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetAllFlights;

public class GetAllFlightsQueryHandler : IRequestHandler<GetAllFlightsQuery, Result<List<FlightDto>>>
{
    private readonly IFlightRepository flightRepository;

    public GetAllFlightsQueryHandler(IFlightRepository flightRepository)
    {
        this.flightRepository = flightRepository;
    }

    public async Task<Result<List<FlightDto>>> Handle(GetAllFlightsQuery request, CancellationToken cancellationToken)
    {
        // get all flights from repository
        var flights = await flightRepository.GetAllAsync();
        // map to FlightDto
        var flightDtos = flights.Select(flight => new FlightDto(
            flight.Id,
            flight.FlightNumber,
            flight.AirlineName,
            flight.DepartureTimeUtc,
            flight.ArrivalTimeUtc,
            flight.OriginCity,
            flight.OriginAirportCode,
            flight.DestinationCity,
            flight.DestinationAirportCode,
            flight.EconomyPrice,
            flight.BusinessPrice,
            flight.FirstClassPrice,
            flight.TotalSeats
        )).ToList();

        return Result<List<FlightDto>>.Success(flightDtos);
    }
}
