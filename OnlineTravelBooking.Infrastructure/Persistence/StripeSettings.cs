using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Infrastructure.Persistence
{
    public class StripeSettings
    {
        public string PublishableKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string WebhookSecret { get; set; } = null!;
    }
}
