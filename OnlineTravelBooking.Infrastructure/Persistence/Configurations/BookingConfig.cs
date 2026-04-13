using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

public class BookingConfig : IEntityTypeConfiguration<BaseBooking>
{
    public void Configure(EntityTypeBuilder<BaseBooking> builder)
    {
        // Primary Key
        builder.HasKey(b => b.Id);

        // Properties
        builder.Property(b => b.BookingReference)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        builder.Property(b => b.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(b => b.RefundAmount)
            .HasPrecision(18, 2);

        builder.Property(b => b.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(b => b.CancellationReason)
            .HasMaxLength(500);

        builder.Property(b => b.Category)
            .HasConversion<int>();

        builder.Property(b => b.Status)
            .HasConversion<int>();

        builder.Property(b => b.PaymentStatus)
            .HasConversion<int>();

        // Indexes
        builder.HasIndex(b => b.BookingReference)
            .IsUnique();

        builder.HasIndex(b => b.UserId);

        builder.HasIndex(b => b.BookingDate);

        builder.HasIndex(b => b.Status);

        builder.HasIndex(b => new { b.UserId, b.Status });

        // Relationships
        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Passengers)
            .WithOne(p => p.Booking)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade); // Delete passengers when booking deleted

        builder.HasMany(b => b.FlightBookings)
            .WithOne(fb => fb.Booking)
            .HasForeignKey(fb => fb.BookingId)
            .OnDelete(DeleteBehavior.Cascade); // Delete flight bookings when booking deleted
    }
}
