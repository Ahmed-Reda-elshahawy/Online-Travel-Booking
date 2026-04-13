using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs.Payment
{
    public record ProcessRefundDto(
     int BookingId,
     int UserId,
     string Reason,
     decimal? PartialAmount = null
 );
}
