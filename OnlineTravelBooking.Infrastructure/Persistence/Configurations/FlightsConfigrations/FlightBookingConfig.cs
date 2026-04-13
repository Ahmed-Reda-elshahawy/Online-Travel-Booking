using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations.FlightsConfigrations;

public class FlightBookingConfig : IEntityTypeConfiguration<FlightBooking>
{
    public void Configure(EntityTypeBuilder<FlightBooking> builder)
    {
        // Primary Key
        builder.HasKey(fb => fb.Id);

        // Properties
        builder.Property(fb => fb.CabinClass)
            .HasConversion<int>();

        builder.Property(fb => fb.Status)
            .HasConversion<int>();

        builder.Property(fb => fb.TicketPrice)
            .HasPrecision(18, 2);

        builder.Property(fb => fb.TaxesAndFees)
            .HasPrecision(18, 2);

        builder.Property(fb => fb.TotalPrice)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(fb => fb.BookingId);

        builder.HasIndex(fb => fb.FlightId);

        builder.HasIndex(fb => fb.PassengerId);

        builder.HasIndex(fb => new { fb.BookingId, fb.FlightId });

        // Relationships
        builder.HasOne(fb => fb.Booking)
            .WithMany(b => b.FlightBookings)
            .HasForeignKey(fb => fb.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fb => fb.Flight)
            .WithMany(f => f.FlightBookings)
            .HasForeignKey(fb => fb.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fb => fb.Passenger)
            .WithMany(p => p.FlightBookings)
            .HasForeignKey(fb => fb.PassengerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
