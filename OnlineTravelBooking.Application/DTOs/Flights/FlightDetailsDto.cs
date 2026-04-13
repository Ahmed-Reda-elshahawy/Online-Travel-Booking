namespace OnlineTravelBooking.Application.DTOs.Flights;

public class FlightDetailsDto
{
    public int Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string AirlineName { get; set; } = string.Empty;
    public string AircraftType { get; set; } = string.Empty;

    public FlightRouteDto Route { get; set; } = null!;
    public FlightScheduleDto Schedule { get; set; } = null!;
    public FlightCapacityDto Capacity { get; set; } = null!;
    public FlightPricingDto Pricing { get; set; } = null!;

    public string Status { get; set; } = string.Empty;
}

public class FlightRouteDto
{
    public AirportInfoDto Origin { get; set; } = null!;
    public AirportInfoDto Destination { get; set; } = null!;
    public double? DistanceKm { get; set; } // Calculated distance

}

public class AirportInfoDto
{
    public string Code { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public LocationDto? Location { get; set; }
}

public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class FlightScheduleDto
{
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Duration { get; set; } = string.Empty;
}

public class FlightCapacityDto
{
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public int BookedSeats { get; set; }
    public int FirstClassRows { get; set; }
    public int BusinessClassRows { get; set; }
    public int EconomyClassRows { get; set; }
    public int SeatsPerRow { get; set; }
}

public class FlightPricingDto
{
    public CabinClassPriceDto Economy { get; set; } = null!;
    public CabinClassPriceDto Business { get; set; } = null!;
    public CabinClassPriceDto First { get; set; } = null!;
}

public class CabinClassPriceDto
{
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }
    public string Currency { get; set; } = "USD";
}