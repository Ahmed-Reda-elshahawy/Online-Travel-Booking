using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Tour
{
    public class TourImage : BaseEntity
    {
        public string ImageUrl { get; set; } = null!;
        public int TourId { get; set; }
        public Tour Tour { get; set; } = null!;
    }
}
