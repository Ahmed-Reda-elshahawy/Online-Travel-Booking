using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.Flights;
using NetTopologySuite.Geometries;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations.FlightsConfigrations;

public class FlightConfig : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        // Primary Key
        builder.HasKey(f => f.Id);

        // Properties
        builder.Property(f => f.FlightNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.AirlineName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.AircraftType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.OriginAirportCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(f => f.OriginCity)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.DestinationAirportCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(f => f.DestinationCity)
            .IsRequired()
            .HasMaxLength(100);

        // Geography Points (SRID 4326 = WGS84 for latitude/longitude)
        builder.Property(f => f.OriginLocation)
            .HasColumnType("geography");

        builder.Property(f => f.DestinationLocation)
            .HasColumnType("geography");

        builder.Property(f => f.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(f => f.EconomyPrice)
            .HasPrecision(18, 2);

        builder.Property(f => f.BusinessPrice)
            .HasPrecision(18, 2);

        builder.Property(f => f.FirstClassPrice)
            .HasPrecision(18, 2);

        builder.Property(f => f.Status)
            .HasConversion<int>();

        // Global query filter for soft delete
        builder.HasQueryFilter(f => !f.IsDeleted);

        // Indexes
        builder.HasIndex(f => f.FlightNumber);

        builder.HasIndex(f => f.DepartureTimeUtc);

        builder.HasIndex(f => f.Status);

        builder.HasIndex(f => new { f.OriginAirportCode, f.DestinationAirportCode, f.DepartureTimeUtc });

        builder.HasIndex(f => new { f.Status, f.DepartureTimeUtc });

        // Relationships
        builder.HasMany(f => f.FlightBookings)
            .WithOne(fb => fb.Flight)
            .HasForeignKey(fb => fb.FlightId)
            .OnDelete(DeleteBehavior.Restrict); // Don't delete bookings when flight deleted
    }
}
