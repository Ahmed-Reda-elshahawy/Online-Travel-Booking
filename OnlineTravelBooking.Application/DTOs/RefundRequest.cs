using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs
{
    public record RefundRequest(
       int BookingId,
       string PaymentIntentId,
       decimal? Amount = null, 
       string? Reason = null
   );
}
