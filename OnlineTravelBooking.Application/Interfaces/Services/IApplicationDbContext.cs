using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Domain.Entities;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Cars;
using OnlineTravelBooking.Domain.Entities.Flights;
using OnlineTravelBooking.Domain.Entities.HotelEntity;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Application.Interfaces.Services;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Hotel> hotels { get; }
    DbSet<HotelImage> hotelImages { get; }
    DbSet<Room> rooms { get; }
    DbSet<RoomImage> roomImages { get; }
    DbSet<Review> reviews { get; }
    DbSet<Favourite> favourites { get; }
    DbSet<BookingRoom> bookingRooms { get; }
    DbSet<BaseBooking> bookings { get; }
    DbSet<Category> categories { get; }
    DbSet<CarBrand> CarBrands { get; }
    DbSet<CarCategory> CarCategories { get; }
    DbSet<RentalLocation> RentalLocations { get; }
    DbSet<Car> Cars { get; }
    DbSet<CarAvailability> CarAvailabilities { get; }
    DbSet<CarImage> CarImages { get; }
    DbSet<CarPricingTier> CarPricingTiers { get; }
    DbSet<CarExtra> CarExtras { get; }
    DbSet<Passenger> Passengers { get; }
    DbSet<Flight> Flights { get; }
    DbSet<FlightBooking> FlightBookings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
