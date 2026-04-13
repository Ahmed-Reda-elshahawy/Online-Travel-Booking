using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities
{
    public class Payment:BaseEntity
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string Gateway { get; set; } = "Stripe";
        public PaymentStatus Status { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public string? ResponseJson { get; set; }
        public string? FailureReason { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? RefundedAt { get; set; }

        // Navigation
        public BaseBooking Booking { get; set; } = null!;

    }
}
