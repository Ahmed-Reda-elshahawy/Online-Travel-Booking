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
    public class CarBrandConfiguration : IEntityTypeConfiguration<CarBrand>
    {
        public void Configure(EntityTypeBuilder<CarBrand> builder)
        {
            builder.ToTable("CarBrand");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true)
                .HasColumnName("Name");

            builder.Property(x => x.LogoUrl)
                .IsRequired()
                .HasColumnName("LogoUrl");

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasColumnName("DisplayOrder");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasColumnName("IsActive");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

           
        }
    }
}
