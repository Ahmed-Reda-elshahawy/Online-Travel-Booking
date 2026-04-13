using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Application.Abstractions
{
    public interface IBookingRepository
    {
		Task<PaginatedList<BaseBooking>> GetAllBookingsAsync(
	   int pageNumber,
	   int pageSize);

		Task<List<BaseBooking>> GetBookingsByUserIdAsync( int userId);
        Task<int> AddBookingAsync(BaseBooking booking);
        Task<BaseBooking> GetBookingDetailsAsync(int bookingId);
        Task UpdateBookingAsync(BaseBooking booking);
    }
}
