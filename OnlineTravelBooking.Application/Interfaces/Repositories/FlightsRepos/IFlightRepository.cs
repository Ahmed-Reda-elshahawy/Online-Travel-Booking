using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;

public interface IFlightRepository
{
    Task<List<Flight>> GetAllAsync();
    Task<Flight?> GetByIdAsync(int id);
    Task<int> AddFlightAsync(Flight flight);
    Task UpdateAsync(Flight flight);
    Task DeleteAsync(Flight flight);
    Task SoftDeleteAsync(int flightId);

    Task<bool> HasBookingsAsync(int flightId);
    Task<int> GetBookingsCountAsync(int flightId);

    Task<bool> IsSeatBookedAsync(int flightId, string seatNumber);
    Task<List<string>> GetBookedSeatsAsync(int flightId);
    Task<List<string>> GetAvailableSeatsAsync(int flightId, int totalSeats);
}
