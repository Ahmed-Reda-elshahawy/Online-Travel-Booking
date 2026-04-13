using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations.CarConfig
{
    public class CarAvailabilityConfiguration : IEntityTypeConfiguration<CarAvailability>
    {
        public void Configure(EntityTypeBuilder<CarAvailability> builder)
        {
            builder.ToTable("CarAvailability");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");

            builder.Property(x => x.CarId)
                .IsRequired()
                .HasColumnName("CarId");

            builder.Property(x => x.LocationId)
                .IsRequired()
                .HasColumnName("LocationId");

            builder.Property(x => x.AvailableFrom)
                .IsRequired()
                .HasColumnName("AvailableFrom");

            builder.Property(x => x.AvailableTo)
                .IsRequired()
                .HasColumnName("AvailableTo");

            builder.Property(x => x.IsAvailable)
                .IsRequired()
                .HasColumnName("IsAvailable");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName("UpdatedAt");

            builder.HasOne(x => x.Car)
                .WithMany(x => x.Availabilities)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Location)
                .WithMany(x => x.CarAvailabilities)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
