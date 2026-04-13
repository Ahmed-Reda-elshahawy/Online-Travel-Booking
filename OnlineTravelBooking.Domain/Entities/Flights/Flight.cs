using OnlineTravelBooking.Domain.Common.Enums.Flight;
using OnlineTravelBooking.Domain.Entities.Base;
using NetTopologySuite.Geometries;

namespace OnlineTravelBooking.Domain.Entities.Flights;

public class Flight : BaseEntity
{
    // Flight Info
    public string FlightNumber { get; set; } = string.Empty; // e.g., "AA123"
    public string AirlineName { get; set; } = string.Empty; // e.g., "American Airlines"
    public string AircraftType { get; set; } = string.Empty; // e.g., "Boeing 737"

    // Route
    public string OriginAirportCode { get; set; } = string.Empty; // e.g., "JFK"
    public string OriginCity { get; set; } = string.Empty;
    public Point? OriginLocation { get; set; }
    
    public string DestinationAirportCode { get; set; } = string.Empty; // e.g., "LAX"
    public string DestinationCity { get; set; } = string.Empty;
    public Point? DestinationLocation { get; set; }

    // Schedule
    public DateTime DepartureTimeUtc { get; set; }
    public DateTime ArrivalTimeUtc { get; set; }
    public int DurationMinutes { get; set; }

    // Capacity
    public int TotalSeats { get; set; }

    // Currency
    public string Currency { get; set; } = "USD";

    // Pricing
    public decimal EconomyPrice { get; set; }
    public decimal BusinessPrice { get; set; }
    public decimal FirstClassPrice { get; set; }

    // Status ( Scheduled, Cancelled, Completed, Departed )
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    // Seat configuration (defines which rows belong to which class)
    public int FirstClassRows { get; set; } = 5;      // Rows 1-5
    public int BusinessClassRows { get; set; } = 5;   // Rows 6-10
    public int EconomyClassRows { get; set; } = 20;   // Rows 11-30
    public int SeatsPerRow { get; set; } = 6; // A-F

    // Soft delete
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    // Navigation
    public ICollection<FlightBooking> FlightBookings { get; set; } = new HashSet<FlightBooking>();
}