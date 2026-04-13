using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Cars
{
    public class CarAvailability: BaseEntity
    {
        public int CarId { get; set; }
        public int LocationId { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public bool IsAvailable { get; set; }
       
      
        // Navigation properties
        public Car Car { get; set; }
        public RentalLocation Location { get; set; }
    }
}
