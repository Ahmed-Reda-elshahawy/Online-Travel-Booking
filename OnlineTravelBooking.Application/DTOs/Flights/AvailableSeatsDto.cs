namespace OnlineTravelBooking.Application.DTOs.Flights;

public class AvailableSeatsDto
{
    public int FlightId { get; set; }
    public int TotalSeats { get; set; }
    public int BookedSeats { get; set; }
    public int AvailableSeats { get; set; }
    public SeatsByClassDto SeatsByClass { get; set; } = new();
}

public class SeatsByClassDto
{
    public List<SeatDto> Economy { get; set; } = new();
    public List<SeatDto> Business { get; set; } = new();
    public List<SeatDto> First { get; set; } = new();
}

public class SeatDto
{
    public string SeatNumber { get; set; } = string.Empty;
    public decimal ExtraCharge { get; set; }
}