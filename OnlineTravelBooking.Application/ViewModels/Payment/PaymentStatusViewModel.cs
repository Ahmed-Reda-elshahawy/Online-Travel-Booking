using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.ViewModels.Payment
{
    public record PaymentStatusViewModel(
      string PaymentIntentId,
      string Status,
      decimal Amount,
      string Currency,
      DateTime? PaidAt
  );
}
