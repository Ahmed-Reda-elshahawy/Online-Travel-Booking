using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.Flights;
using OnlineTravelBooking.Domain.Entities.User;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Domain.Entities.Booking;

public class BaseBooking : BaseEntity
{
    // FKs
    public int UserId { get;  set; }
    public int? HotelId { get; set; }
    public int? CarId { get; set; }
    public int? TourId { get; set; }

    // Reference
    public string BookingReference { get; set; } = string.Empty;

    // Booking info
    public BookingCategory Category { get;  set; }
    public BookingStatus Status { get;  set; }
    public decimal TotalPrice { get;  set; }
    public string Currency { get;  set; } = null!;
    public DateTime BookingDate { get;  set; }

    // Payment
    public PaymentStatus PaymentStatus { get;  set; }
    public string PaymentMethod { get;  set; } = string.Empty;

    // Cancellation / Refund
    public DateTime? CancelledAt { get;  set; }
    public string? CancellationReason { get;  set; }
    public decimal? RefundAmount { get;  set; }
    public DateTime? RefundProcessedAt { get; set; }

    // Navigation properties
    public Review? review { get;  set; }
    public ApplicationUser User { get; set; } = null!;
    public ICollection<Passenger> Passengers { get; set; } = new HashSet<Passenger>();
    public ICollection<FlightBooking> FlightBookings { get; set; } = new HashSet<FlightBooking>();
    public ICollection<BookingRoom> BookingRoom { get; set; } = new List<BookingRoom>();
}
