using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.Flights;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations.FlightsConfigrations;

public class PassengerConfig : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.PassportNumber)
            .HasMaxLength(50);

        builder.Property(p => p.MealPreference)
            .HasMaxLength(100);

        builder.Property(p => p.SpecialAssistance)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(p => p.BookingId);

        builder.HasIndex(p => p.Email);

        builder.HasIndex(p => p.PassportNumber);

        // Relationships
        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Passengers)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.FlightBookings)
            .WithOne(fb => fb.Passenger)
            .HasForeignKey(fb => fb.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
