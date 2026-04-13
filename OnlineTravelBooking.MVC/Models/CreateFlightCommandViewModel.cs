using System.ComponentModel.DataAnnotations;

namespace OnlineTravelBooking.MVC.Models;

public class CreateFlightCommandViewModel
{
    [Required]
    [StringLength(20)]
    public string FlightNumber { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string AirlineName { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string AircraftType { get; init; } = string.Empty;

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string OriginAirportCode { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string OriginCity { get; init; } = string.Empty;

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string DestinationAirportCode { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string DestinationCity { get; init; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime DepartureTimeUtc { get; init; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime ArrivalTimeUtc { get; init; }

    [Range(0, 100)]
    public int FirstClassRows { get; init; }

    [Range(0, 100)]
    public int BusinessClassRows { get; init; }

    [Range(0, 500)]
    public int EconomyClassRows { get; init; }

    public int TotalSeats { get; init; }

    public int SeatsPerRow { get; init; } = 6;

    [Required]
    public string Currency { get; init; } = "USD";

    [Range(0, double.MaxValue)]
    public decimal EconomyPrice { get; init; }

    [Range(0, double.MaxValue)]
    public decimal BusinessPrice { get; init; }

    [Range(0, double.MaxValue)]
    public decimal FirstClassPrice { get; init; }
}
