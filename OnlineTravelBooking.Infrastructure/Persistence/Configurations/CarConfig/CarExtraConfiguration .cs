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
    public class CarExtraConfiguration : IEntityTypeConfiguration<CarExtra>
    {
        public void Configure(EntityTypeBuilder<CarExtra> builder)
        {
            builder.ToTable("CarExtra");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Name");

            builder.Property(x => x.Description)
                .IsRequired()
                .HasColumnName("Description");

            builder.Property(x => x.Price)
                .IsRequired()
                .HasPrecision(10, 2)
                .HasColumnName("Price");

            builder.Property(x => x.PricingType)
                .IsRequired()
                .HasColumnName("PricingType")
                .HasConversion<string>();

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("Currency");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasColumnName("IsActive");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");
        }
    }
}
