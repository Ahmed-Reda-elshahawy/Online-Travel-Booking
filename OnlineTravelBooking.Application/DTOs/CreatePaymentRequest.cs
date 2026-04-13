using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs
{
    public record CreatePaymentRequest(
    int BookingId,
    decimal Amount,
    string Currency,
    Dictionary<string, string>? Metadata = null
);
}
