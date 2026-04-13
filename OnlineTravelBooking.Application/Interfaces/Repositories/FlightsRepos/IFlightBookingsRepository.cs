using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;

public interface IFlightBookingsRepository
{
    Task<BaseBooking> CreateBookingAsync(BaseBooking booking);
    Task CreateFlightBookingAsync(FlightBooking flightBooking);
    Task<BaseBooking?> GetBookingByIdAsync(int id);
    Task<List<BaseBooking>> GetUserBookingsAsync(int userId);
    //Task<List<BaseBooking>> GetBookingsByFlightIdAsync(int flightId);
    //Task DeleteAsync(BaseBooking flightBooking);
}
