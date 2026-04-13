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
    public class CarImageConfiguration : IEntityTypeConfiguration<CarImage>
    {
        public void Configure(EntityTypeBuilder<CarImage> builder)
        {
            builder.ToTable("CarImage");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id");

            builder.Property(x => x.CarId)
                .IsRequired()
                .HasColumnName("CarId");

            builder.Property(x => x.Url)
                .IsRequired()
                .HasColumnName("Url");

            builder.Property(x => x.Caption)
                .IsRequired()
                .HasColumnName("Caption");

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasColumnName("DisplayOrder");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.HasOne(x => x.Car)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
