using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Domain.Common.Enums.Flight;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.CreateFlight;

public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, Result<string>>
{
    private readonly IFlightRepository flightRepository;
    private readonly GeometryFactory geometryFactory;

    public CreateFlightCommandHandler(IFlightRepository flightRepository)
    {
        this.flightRepository = flightRepository;
        this.geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
    }

    public async Task<Result<string>> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        // create point locations
        Point? originLocation = null;
        Point? destinationLocation = null;
        if (request.OriginLatitude.HasValue && request.OriginLongitude.HasValue)
        {
            originLocation = geometryFactory.CreatePoint(new Coordinate(request.OriginLongitude.Value, request.OriginLatitude.Value));
        }
        if (request.DestinationLatitude.HasValue && request.DestinationLongitude.HasValue)
        {
            destinationLocation = geometryFactory.CreatePoint(new Coordinate(request.DestinationLongitude.Value, request.DestinationLatitude.Value));
        }

        // Calculate duration
        var duration = request.ArrivalTimeUtc - request.DepartureTimeUtc;
        var durationMinutes = (int)duration.TotalMinutes;

        // Seats per row fixed at 6
        var seatsPerRow = 6;

        var totalSeatRows = request.EconomyClassRows + request.BusinessClassRows + request.FirstClassRows;

        var flight = new Flight
        {
            FlightNumber = request.FlightNumber.ToUpper(),
            AirlineName = request.AirlineName,
            AircraftType = request.AircraftType,

            OriginAirportCode = request.OriginAirportCode.ToUpper(),
            OriginCity = request.OriginCity,
            OriginLocation = originLocation,

            DestinationAirportCode = request.DestinationAirportCode.ToUpper(),
            DestinationCity = request.DestinationCity,
            DestinationLocation = destinationLocation,

            DepartureTimeUtc = request.DepartureTimeUtc,
            ArrivalTimeUtc = request.ArrivalTimeUtc,
            DurationMinutes = durationMinutes,

            // Set seat configuration
            FirstClassRows = request.FirstClassRows,
            BusinessClassRows = request.BusinessClassRows,
            EconomyClassRows = request.EconomyClassRows,
            SeatsPerRow = seatsPerRow,

            TotalSeats = totalSeatRows * seatsPerRow,

            Currency = request.Currency,

            EconomyPrice = request.EconomyPrice,
            BusinessPrice = request.BusinessPrice,
            FirstClassPrice = request.FirstClassPrice,

            Status = FlightStatus.Scheduled,
            CreatedAt = DateTime.UtcNow
        };
        var createdFlightId = await flightRepository.AddFlightAsync(flight);

        return Result.Success($"Flight of Id : {createdFlightId} Created Successfully");
    }
}
