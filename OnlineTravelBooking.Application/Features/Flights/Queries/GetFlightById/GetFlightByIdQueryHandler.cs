using MediatR;
using OnlineTravelBooking.Application.Common.Helpers.Flights;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.DTOs.Flights;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Domain.Common.Enums.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Queries.GetFlightById;

public class GetFlightByIdQueryHandler : IRequestHandler<GetFlightByIdQuery, Result<FlightDetailsDto>>
{
    private readonly IFlightRepository flightRepository;

    public GetFlightByIdQueryHandler(IFlightRepository flightRepository)
    {
        this.flightRepository = flightRepository;
    }

    public async Task<Result<FlightDetailsDto>> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
    {
        // Fetch flight by ID from the repository and check if it exists
        var flight = await flightRepository.GetByIdAsync(request.FlightId);
        if (flight == null)
            return Result.Failure<FlightDetailsDto>(new Error("FlightNotFound", $"Flight with ID {request.FlightId} not found."));
        
        var bookedSeats = await flightRepository.GetBookedSeatsAsync(request.FlightId);
        var availableSeats = SeatHelper.GetAvailableSeatsWithClass(flight, bookedSeats);

        // Calculated variables
        var duration = flight.ArrivalTimeUtc - flight.DepartureTimeUtc;
        var formatedDuration = new TimeSpan(duration.Days, duration.Hours, duration.Minutes, duration.Seconds).ToString();



        // Map flight entity to FlightDto
        var Dto = new FlightDetailsDto
        {
            Id = request.FlightId,
            FlightNumber = flight.FlightNumber,
            AirlineName = flight.AirlineName,
            AircraftType = flight.AircraftType,
            Route = new FlightRouteDto
            {
                Origin = new AirportInfoDto
                {
                    Code = flight.OriginAirportCode,
                    City = flight.OriginCity,
                    Location = flight.OriginLocation != null ? new LocationDto
                    {
                        Latitude = flight.OriginLocation.Y,
                        Longitude = flight.OriginLocation.X
                    } : null
                },
                Destination = new AirportInfoDto 
                { 
                    Code= flight.DestinationAirportCode,
                    City = flight.DestinationCity,
                    Location = flight.DestinationLocation != null ? new LocationDto
                    {
                        Latitude = flight.DestinationLocation.Y,
                        Longitude = flight.DestinationLocation.X
                    } : null
                },
                DistanceKm = FlightsHelpers.CalculateDistance(flight.OriginLocation, flight.DestinationLocation)
            },
            Schedule = new FlightScheduleDto
            {
                DepartureTime = flight.DepartureTimeUtc,
                ArrivalTime = flight.ArrivalTimeUtc,
                Duration = formatedDuration
            },
            Capacity = new FlightCapacityDto
            {
                TotalSeats = flight.TotalSeats,
                AvailableSeats = availableSeats.Count,
                BookedSeats = flight.TotalSeats - availableSeats.Count
            },
            Pricing = new FlightPricingDto
            {
                Economy = new CabinClassPriceDto
                {
                    Price = flight.EconomyPrice,
                    AvailableSeats = availableSeats.Count(s => s.CabinClass == CabinClass.Economy),
                    Currency = flight.Currency
                },
                Business = new CabinClassPriceDto
                {
                    Price = flight.BusinessPrice,
                    AvailableSeats = availableSeats.Count(s => s.CabinClass == CabinClass.Business),
                    Currency = flight.Currency
                },
                First = new CabinClassPriceDto
                {
                    Price = flight.FirstClassPrice,
                    AvailableSeats = availableSeats.Count(s => s.CabinClass == CabinClass.First),
                    Currency = flight.Currency
                }
            },
            Status = flight.Status.ToString(),
        };

        return Result.Success(Dto);
    }
}
