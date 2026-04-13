using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.ViewModels.Payment
{
    public record PaymentIntentViewModel(
       string PaymentIntentId,
       string ClientSecret,
       decimal Amount,
       string Currency,
       string Status
   );
}
