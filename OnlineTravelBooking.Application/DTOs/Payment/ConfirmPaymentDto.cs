using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs.Payment
{
    public record ConfirmPaymentDto(
      int BookingId,
      string PaymentIntentId
  );
}
