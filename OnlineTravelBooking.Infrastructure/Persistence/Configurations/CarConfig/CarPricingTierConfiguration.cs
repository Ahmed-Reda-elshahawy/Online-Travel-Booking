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
    public class CarPricingTierConfiguration : IEntityTypeConfiguration<CarPricingTier>
    {
        public void Configure(EntityTypeBuilder<CarPricingTier> builder)
        {
            builder.ToTable("CarPricingTier");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");

            builder.Property(x => x.CarId)
                .IsRequired()
                .HasColumnName("CarId");

            builder.Property(x => x.TierName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("TierName");

            builder.Property(x => x.FromHours)
                .IsRequired()
                .HasColumnName("FromHours");

            builder.Property(x => x.ToHours)
                .HasColumnName("ToHours");

            builder.Property(x => x.PricePerHour)
                .IsRequired()
                .HasPrecision(10, 2)
                .HasColumnName("PricePerHour");

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("Currency");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName("UpdatedAt");

            builder.HasOne(x => x.Car)
                .WithMany(x => x.PricingTiers)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
