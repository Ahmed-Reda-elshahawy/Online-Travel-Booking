using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.ViewModels.Payment
{
    public record PaymentConfirmationViewModel(string PaymentIntentId,
    string Status,
    decimal Amount,
    DateTime? PaidAt,
    string? TransactionId);
    
}
