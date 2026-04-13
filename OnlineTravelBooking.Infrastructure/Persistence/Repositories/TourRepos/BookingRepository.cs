using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Abstractions;
using OnlineTravelBooking.Application.Common.Models;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Tour;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.TourRepos;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _dbContext;
    public BookingRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
	public async Task<PaginatedList<BaseBooking>> GetAllBookingsAsync(int pageNumber, int pageSize)
	{
		var query = _dbContext.bookings
		   .AsNoTracking()
		   .OrderByDescending(b => b.BookingDate);

		return await PaginatedList<BaseBooking>.Create(query, pageNumber, pageSize);
	}

	public async Task<List<BaseBooking>> GetBookingsByUserIdAsync(int userId)
	{
		var query = await _dbContext.bookings
			.AsNoTracking()
		   .Where(b => b.UserId == userId)
		   .OrderByDescending(b => b.BookingDate)
		   .ToListAsync() ;

		return query;
	}
    
    public async Task<int> AddBookingAsync(BaseBooking booking)
    {
        await  _dbContext.bookings.AddAsync(booking);
        if(booking.TourId.HasValue)
        {
            var tour = await _dbContext.Set<Tour>().FindAsync(booking.TourId.Value);
            if(tour != null)
            {
                tour.BookingCount += 1;
                _dbContext.Set<Tour>().Update(tour);
            }
        }
        await _dbContext.SaveChangesAsync();
        return booking.Id;
    }

    public async Task<BaseBooking> GetBookingDetailsAsync(int bookingId)
    {
        return await _dbContext.bookings.FindAsync(bookingId);
    }

    public async Task UpdateBookingAsync(BaseBooking booking)
    {
        _dbContext.bookings.Update(booking);
        await _dbContext.SaveChangesAsync();
    }
}
	
