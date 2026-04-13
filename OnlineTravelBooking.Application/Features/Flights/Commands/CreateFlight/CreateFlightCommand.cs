using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.CreateFlight;

public record CreateFlightCommand : IRequest<Result<string>>
{
    public string FlightNumber { get; init; } = string.Empty;
    public string AirlineName { get; init; } = string.Empty;
    public string AircraftType { get; init; } = string.Empty;

    // Origin
    public string OriginAirportCode { get; init; } = string.Empty;
    public string OriginCity { get; init; } = string.Empty;
    public double? OriginLatitude { get; init; }
    public double? OriginLongitude { get; init; }

    // Destination
    public string DestinationAirportCode { get; init; } = string.Empty;
    public string DestinationCity { get; init; } = string.Empty;
    public double? DestinationLatitude { get; init; }
    public double? DestinationLongitude { get; init; }

    public DateTime DepartureTimeUtc { get; init; }
    public DateTime ArrivalTimeUtc { get; init; }

    // Seat layout configuration
    public int FirstClassRows { get; init; } = 5;
    public int BusinessClassRows { get; init; } = 5;
    public int EconomyClassRows { get; init; } = 20;

    public string Currency { get; init; } = "USD";

    public decimal EconomyPrice { get; init; }
    public decimal BusinessPrice { get; init; }
    public decimal FirstClassPrice { get; init; }
}

