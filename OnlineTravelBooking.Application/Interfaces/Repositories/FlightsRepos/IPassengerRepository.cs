using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;

public interface IPassengerRepository
{
    Task AddPassengerAsync(Passenger passenger);
}
