using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.ViewModels.Payment
{
    public record RefundViewModel(
     string RefundId,
     decimal Amount,
     string Status,
     DateTime RefundedAt
 );
}
