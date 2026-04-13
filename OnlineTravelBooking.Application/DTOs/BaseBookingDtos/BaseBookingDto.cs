namespace OnlineTravelBooking.Application.DTOs.BaseBookingDtos;

public class BaseBookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? RefundAmount { get; set; }
    public DateTime? RefundProcessedAt { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
}
