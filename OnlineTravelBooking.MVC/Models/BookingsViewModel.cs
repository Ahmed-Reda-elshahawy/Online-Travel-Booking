namespace OnlineTravelBooking.MVC.Models;

public class BookingsViewModel
{
    public List<BookingItemViewModel> Bookings { get; set; } = new();
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class BookingItemViewModel
{
    public int Id { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public DateTime? CancelledAt { get; set; }
    public string? CancellationReason { get; set; }
    public decimal? RefundAmount { get; set; }
}
