using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Cars
{
    public class CarImage : BaseEntity
    {
        public int CarId { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public int DisplayOrder { get; set; }
      

        // Navigation properties
        public Car Car { get; set; }
    }
}
