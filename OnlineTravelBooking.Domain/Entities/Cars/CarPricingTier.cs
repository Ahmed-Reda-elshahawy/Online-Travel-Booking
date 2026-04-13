using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Cars
{
    public class CarPricingTier: BaseEntity
    {
        public int CarId { get; set; }
        public string TierName { get; set; }
        public int FromHours { get; set; }
        public int? ToHours { get; set; }
        public decimal PricePerHour { get; set; }
        public string Currency { get; set; }
      


        public Car Car { get; set; }
    }
}
