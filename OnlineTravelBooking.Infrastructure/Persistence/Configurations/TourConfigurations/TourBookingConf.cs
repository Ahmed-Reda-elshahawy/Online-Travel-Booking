using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.Tour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations.TourConfigurations
{
    public class TourBookingConf : IEntityTypeConfiguration<BookingTour>
    {
        public void Configure(EntityTypeBuilder<BookingTour> builder)
        {
             builder.Property(bt => bt.unitPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
            builder.HasOne(bt => bt.TourSchedule)
                   .WithMany()
                   .HasForeignKey(bt => bt.TourScheduleId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(bt => bt.NumberOfGuests)
                   .IsRequired();
            builder.HasOne(bt => bt.Booking)
                   .WithMany()
                   .HasForeignKey(bt => bt.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
