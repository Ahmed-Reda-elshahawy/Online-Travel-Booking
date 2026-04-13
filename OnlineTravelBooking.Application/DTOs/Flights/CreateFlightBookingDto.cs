using OnlineTravelBooking.Domain.Common.Enums.Flights;

namespace OnlineTravelBooking.Application.DTOs.Flights;

public class CreateFlightBookingRequest
{
    public int FlightId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public List<PassengerDto> Passengers { get; set; } = new();
}

public class PassengerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? PassportNumber { get; set; }
    public CabinClass CabinClass { get; set; }
    public string? SeatNumber { get; set; }
    public string? MealPreference { get; set; }
}

public class CreateFlightBookingResponse
{
    public int BookingId { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}