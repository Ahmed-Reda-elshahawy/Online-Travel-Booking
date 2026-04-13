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
    public class RentalLocationConfiguration : IEntityTypeConfiguration<RentalLocation>
    {
        public void Configure(EntityTypeBuilder<RentalLocation> builder)
        {
            builder.ToTable("RentalLocation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Name");

            builder.Property(x => x.Address)
                .IsRequired()
                .HasColumnName("Address");

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("City");

            builder.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Country");

            builder.Property(x => x.Latitude)
                .IsRequired()
                .HasPrecision(10, 8)
                .HasColumnName("Latitude");

            builder.Property(x => x.Longitude)
                .IsRequired()
                .HasPrecision(11, 8)
                .HasColumnName("Longitude");

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Phone");

            builder.Property(x => x.OperatingHours)
                .IsRequired()
                .HasColumnName("OperatingHours");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasColumnName("IsActive");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.HasMany(x => x.CarAvailabilities)
                .WithOne(x => x.Location)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
