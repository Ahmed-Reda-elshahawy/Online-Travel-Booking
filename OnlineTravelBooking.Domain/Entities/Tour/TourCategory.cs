using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Tour
{
    public class TourCategory : BaseEntity
    {
        public string Name { get; set; } = null!;
        
    }
}
