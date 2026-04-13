using MediatR;
using OnlineTravelBooking.Application.Common.Models;

namespace OnlineTravelBooking.Application.Features.Flights.Commands.UpdateFlight;

public record UpdateFlightCommand : IRequest<Result>
{
    public int Id { get; init; }
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

    // Schedule
    public DateTime DepartureTimeUtc { get; init; }
    public DateTime ArrivalTimeUtc { get; init; }

    // Capacity & Pricing
    public int TotalSeats { get; init; }
    public int FirstClassRows { get; init; }
    public int BusinessClassRows { get; init; }
    public int EconomyClassRows { get; init; }
    public int SeatsPerRow { get; init; }

    public string Currency { get; init; } = "USD";
    public decimal EconomyPrice { get; init; }
    public decimal BusinessPrice { get; init; }
    public decimal FirstClassPrice { get; init; }

    // Status
    public string Status { get; init; } = string.Empty;
}