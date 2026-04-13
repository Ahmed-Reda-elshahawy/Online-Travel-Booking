using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs.TourDtos
{
    public class BookingContDto
    {
        public int? Id { get; set; }
        public int TourScheduleId { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal unitPrice { get; set; }
        public int BookingId { get; set; }
        public int? TourId { get; set; }
        public int Category { get; set; }
        public int Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = null!;
        public DateTime BookingDate { get; set; }


        public int PaymentStatus { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
        public decimal? RefundAmount { get; set; }
        public DateTime? RefundProcessedAt { get; set; }
    }
}
