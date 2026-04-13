using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Cars
{
    public class CarBrand: BaseEntity
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public ICollection<Car> Cars { get; set; }

    }
}
