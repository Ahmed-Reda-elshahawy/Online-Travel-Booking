using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Common.Enums.Flights;
using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.Domain.Entities.Flights;

public class FlightBooking : BaseEntity
{
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public int PassengerId { get; set; }

    // Class & Seat
    public CabinClass CabinClass { get; set; } // Economy, Business, First
    public string SeatNumber { get; set; } = string.Empty; // "12A"

    // Pricing
    public decimal TicketPrice { get; set; }
    public decimal TaxesAndFees { get; set; }
    public decimal TotalPrice { get; set; }

    // Status
    public BookingStatus Status { get; set; } = BookingStatus.Pending; // Confirmed, Cancelled, Pending

    // Navigation
    public BaseBooking Booking { get; set; } = null!;
    public Flight Flight { get; set; } = null!;
    public Passenger Passenger { get; set; } = null!;
}

