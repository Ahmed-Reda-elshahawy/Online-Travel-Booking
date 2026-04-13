using OnlineTravelBooking.Application.Interfaces.Repositories.FlightsRepos;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Infrastructure.Persistence.Repositories.FlightsRepos;

public class PassengerRepository : IPassengerRepository
{
    private readonly IApplicationDbContext dbContext;

    public PassengerRepository(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddPassengerAsync(Passenger passenger)
    {
        await dbContext.Passengers.AddAsync(passenger);    
        await dbContext.SaveChangesAsync();
    }
}
