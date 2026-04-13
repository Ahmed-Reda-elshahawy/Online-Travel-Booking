using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.FlightsRepos;

public class FlightBookingsRepository : IFlightBookingsRepository
{
    private readonly IApplicationDbContext dbContext;

    public FlightBookingsRepository(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<BaseBooking> CreateBookingAsync(BaseBooking booking)
    {
        await dbContext.bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();
        return booking;
    }

    public async Task CreateFlightBookingAsync(FlightBooking flightBooking)
    {
        await dbContext.FlightBookings.AddAsync(flightBooking);
        await dbContext.SaveChangesAsync();
    }

    public async Task<BaseBooking?> GetBookingByIdAsync(int id)
    {
        return await dbContext.bookings
            .Include(b => b.Passengers)
            .Include(b => b.FlightBookings)
                .ThenInclude(fb => fb.Flight)
            .Include(b => b.FlightBookings)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public Task<List<BaseBooking>> GetUserBookingsAsync(int userId)
    {
        return dbContext.bookings
            .Include(b => b.FlightBookings)
                .ThenInclude(fb => fb.Flight)
            .Where(b => b.UserId == userId && b.Category == BookingCategory.Flight)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }
}
