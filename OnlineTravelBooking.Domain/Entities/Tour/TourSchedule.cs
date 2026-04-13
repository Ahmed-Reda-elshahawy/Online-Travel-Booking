using OnlineTravelBooking.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Domain.Entities.Tour
{
    public class TourSchedule : BaseEntity
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailableSeats { get; set; }
    }
}
