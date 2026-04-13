using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Application.Interfaces.Services;
using OnlineTravelBooking.Domain.Entities;
using OnlineTravelBooking.Domain.Entities.Booking;
using OnlineTravelBooking.Domain.Entities.Cars;
using OnlineTravelBooking.Domain.Entities.Flights;
using OnlineTravelBooking.Domain.Entities.HotelEntity;
using OnlineTravelBooking.Domain.Entities.Tour;
using OnlineTravelBooking.Domain.Entities.User;
using System.Reflection;

namespace OnlineTravelBooking.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public new DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Hotel> hotels => Set<Hotel>();
    public DbSet<HotelImage> hotelImages => Set<HotelImage>();
    public DbSet<Room> rooms => Set<Room>();
    public DbSet<RoomImage> roomImages => Set<RoomImage>();
    public DbSet<Review> reviews => Set<Review>();
    public DbSet<Favourite> favourites => Set<Favourite>();
    public DbSet<BookingRoom> bookingRooms => Set<BookingRoom>();
    public DbSet<BaseBooking> bookings => Set<BaseBooking>();
    public DbSet<CarBrand> CarBrands => Set<CarBrand>();
    public DbSet<CarCategory> CarCategories => Set<CarCategory>();
    public DbSet<RentalLocation> RentalLocations => Set<RentalLocation>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<CarAvailability> CarAvailabilities => Set<CarAvailability>();
    public DbSet<CarImage> CarImages => Set<CarImage>();
    public DbSet<CarPricingTier> CarPricingTiers => Set<CarPricingTier>();
    public DbSet<CarExtra> CarExtras => Set<CarExtra>();
    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<FlightBooking> FlightBookings => Set<FlightBooking>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<BookingTour> BookingTours => Set<BookingTour>();

	public DbSet<Category> categories => Set<Category>();

	protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
