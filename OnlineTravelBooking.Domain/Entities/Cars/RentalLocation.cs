using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Cars
{
    public class RentalLocation: BaseEntity
    {
       public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Phone { get; set; }
        public string OperatingHours { get; set; }
        public bool IsActive { get; set; }
    
        // Navigation properties
        public ICollection<CarAvailability> CarAvailabilities { get; set; }
    }
}
