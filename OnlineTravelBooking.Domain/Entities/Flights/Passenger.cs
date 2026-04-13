using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.Domain.Entities.Flights;

public class Passenger : BaseEntity
{
    public int BookingId { get; set; }

    // Personal Info
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Travel Document
    public string? PassportNumber { get; set; }

    // Special Requests
    public string? MealPreference { get; set; }
    public string? SpecialAssistance { get; set; }

    // Navigation
    public BaseBooking Booking { get; set; } = null!;
    public ICollection<FlightBooking> FlightBookings { get; set; } = new HashSet<FlightBooking>();
}