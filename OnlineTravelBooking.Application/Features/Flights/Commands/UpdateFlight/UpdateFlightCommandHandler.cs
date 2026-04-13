using MediatR;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Domain.Common.Enums.Flight;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.UpdateFlight;

public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, Result>
{
    private readonly IFlightRepository flightRepository;
    private readonly GeometryFactory geometryFactory;

    public UpdateFlightCommandHandler(IFlightRepository flightRepository)
    {
        this.flightRepository = flightRepository;
        geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
    }

    public async Task<Result> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
    {
        // Get existing flight
        var flight = await flightRepository.GetByIdAsync(request.Id);
        if (flight == null)
            return Result.Failure(new Error("FlightNotFound", $"Flight with ID {request.Id} not found."));

        // Check if flight has bookings (optional - prevent changes if already booked)
        var hasBookings = await flightRepository.HasBookingsAsync(request.Id);
        if (hasBookings)
            return Result.Failure(new Error("FlightHasBookings", "Cannot update flight that already has bookings. Consider creating a new flight instead."));

        // Calculate duration
        var duration = request.ArrivalTimeUtc - request.DepartureTimeUtc;
        var durationMinutes = (int)duration.TotalMinutes;

        // Create Point locations
        Point? originLocation = null;
        if (request.OriginLatitude.HasValue && request.OriginLongitude.HasValue)
        {
            originLocation = geometryFactory.CreatePoint(new Coordinate(request.OriginLongitude.Value, request.OriginLatitude.Value));
        }

        Point? destinationLocation = null;
        if (request.DestinationLatitude.HasValue && request.DestinationLongitude.HasValue)
        {
            destinationLocation = geometryFactory.CreatePoint(new Coordinate(request.DestinationLongitude.Value, request.DestinationLatitude.Value));
        }

        // Parse status
        if (!Enum.TryParse<FlightStatus>(request.Status, out var flightStatus))
            return Result.Failure(new Error("InvalidStatus", $"Invalid flight status: {request.Status}"));

        // Update flight properties
        flight.FlightNumber = request.FlightNumber.ToUpper();
        flight.AirlineName = request.AirlineName;
        flight.AircraftType = request.AircraftType;

        flight.OriginAirportCode = request.OriginAirportCode.ToUpper();
        flight.OriginCity = request.OriginCity;
        flight.OriginLocation = originLocation;

        flight.DestinationAirportCode = request.DestinationAirportCode.ToUpper();
        flight.DestinationCity = request.DestinationCity;
        flight.DestinationLocation = destinationLocation;

        flight.DepartureTimeUtc = request.DepartureTimeUtc;
        flight.ArrivalTimeUtc = request.ArrivalTimeUtc;
        flight.DurationMinutes = durationMinutes;

        // Seats per row fixed at 6
        var seatsPerRow = 6;
        flight.FirstClassRows = request.FirstClassRows;
        flight.BusinessClassRows = request.BusinessClassRows;
        flight.EconomyClassRows = request.EconomyClassRows;
        flight.SeatsPerRow = seatsPerRow;

        flight.TotalSeats = (request.FirstClassRows + request.BusinessClassRows + request.EconomyClassRows) * seatsPerRow;
        flight.Currency = request.Currency.ToUpper();

        flight.EconomyPrice = request.EconomyPrice;
        flight.BusinessPrice = request.BusinessPrice;
        flight.FirstClassPrice = request.FirstClassPrice;

        flight.Status = flightStatus;

        // Save changes
        await flightRepository.UpdateAsync(flight);

        return Result.Success();
    }
}
