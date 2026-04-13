using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.DTOs.CarDtos
{
    public class CarSearchDto
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Type { get; set; }
        public string? TransmissionType { get; set; }
        public string? FuelType { get; set; }
        public int? MinSeats { get; set; }
        public int? MaxSeats { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
