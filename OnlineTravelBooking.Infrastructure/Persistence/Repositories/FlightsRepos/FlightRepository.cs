using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Common.Helpers.Flights;
using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Common.Enums.Booking;
using OnlineTravelBooking.Domain.Common.Enums.Flights;
using OnlineTravelBooking.Domain.Entities.Flights;
using System.Threading;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.FlightsRepos;

public class FlightRepository : IFlightRepository
{
    private readonly IApplicationDbContext dbContext;

    public FlightRepository(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<int> AddFlightAsync(Flight flight)
    {
        var entityEntry = await dbContext.Flights.AddAsync(flight);
        await dbContext.SaveChangesAsync();
        return entityEntry.Entity.Id;
    }

    public async Task DeleteAsync(Flight flight)
    {
        dbContext.Flights.Remove(flight);
        await dbContext.SaveChangesAsync();
    }

    public Task<int> GetBookingsCountAsync(int flightId)
    {
        return dbContext.FlightBookings.CountAsync(fb => fb.FlightId == flightId);
    }

    public async Task<Flight?> GetByIdAsync(int id)
    {
        return await dbContext.Flights.FindAsync(id);
    }

    public async Task<bool> HasBookingsAsync(int flightId)
    {
        return await dbContext.FlightBookings.AnyAsync(fb => fb.FlightId == flightId);
    }

    public async Task SoftDeleteAsync(int flightId)
    {
        var flight = await dbContext.Flights.IgnoreQueryFilters().FirstOrDefaultAsync(f => f.Id == flightId);
        
        if (flight == null)
            throw new InvalidOperationException($"Flight with ID {flightId} not found.");

        if (flight.IsDeleted)
            throw new InvalidOperationException($"Flight with ID {flightId} is already deleted.");

        flight.IsDeleted = true;
        flight.DeletedAt = DateTime.Now;
        dbContext.Flights.Update(flight);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Flight flight)
    {
        flight.UpdatedAt = DateTime.Now;
        dbContext.Flights.Update(flight);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<string>> GetAvailableSeatsAsync(int flightId, int totalSeats)
    {
        var flight = await dbContext.Flights.FindAsync(flightId);

        // Get booked seats
        var bookedSeats = await GetBookedSeatsAsync(flightId);

        // Generate all possible seats (example: 1A to 30F for 180 seats)
        var allSeats = SeatHelper.GenerateAllSeats(flight!);

        // Return seats that are not booked
        return allSeats.Except(bookedSeats).ToList();
    }

    public async Task<List<string>> GetBookedSeatsAsync(int flightId)
    {
        return await dbContext.FlightBookings
            .Where(fb => fb.FlightId == flightId && fb.SeatNumber != null && fb.Status != BookingStatus.Cancelled)
            .Select(fb => fb.SeatNumber)
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> IsSeatBookedAsync(int flightId, string seatNumber)
    {
        return await dbContext.FlightBookings
            .AnyAsync(fb => fb.FlightId == flightId && fb.SeatNumber == seatNumber && fb.Status != BookingStatus.Cancelled);
    }

    public async Task<List<Flight>> GetAllAsync()
    {
        return await dbContext.Flights.ToListAsync();
    }
}