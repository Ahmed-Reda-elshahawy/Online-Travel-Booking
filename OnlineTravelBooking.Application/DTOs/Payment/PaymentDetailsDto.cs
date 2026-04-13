using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs.Payment
{
    public record PaymentDetailsDto(
     int PaymentId,
     int BookingId,
     decimal Amount,
     string Currency,
     string Status,
     string? TransactionId,
     DateTime? PaidAt,
     DateTime? RefundedAt
 );
}
