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
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Car");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");

           

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Model");

            builder.Property(x => x.Year)
                .HasColumnName("Year");

           

            builder.Property(x => x.FuelType)
                .HasColumnName("FuelType")
                .HasConversion<string>();

           

            

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName("UpdatedAt");

            

            builder.HasMany(x => x.Availabilities)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Images)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.PricingTiers)
                .WithOne(x => x.Car)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


