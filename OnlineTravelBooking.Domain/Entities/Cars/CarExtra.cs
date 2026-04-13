using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Cars
{
    public enum ExtraPricingType
    {
        PerDay=1,
        PerHour=2,
        OneTime=3,
        PerWeek=4
    }

    public class CarExtra: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ExtraPricingType PricingType { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
    }
}
