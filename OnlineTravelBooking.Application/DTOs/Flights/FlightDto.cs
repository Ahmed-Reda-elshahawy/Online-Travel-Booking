namespace OnlineTravelBooking.Application.DTOs.Flights;

public record FlightDto(
    int Id,
    string FlightNumber,
    string AirlineName,
    DateTime DepartureTimeUtc,
    DateTime ArrivalTimeUtc,
    string OriginCity,
    string OriginAirportCode,
    string DestinationCity,
    string DestinationAirportCode,
    decimal EconomyPrice,
    decimal BusinessPrice,
    decimal FirstClassPrice,
    int TotalSeats
);
