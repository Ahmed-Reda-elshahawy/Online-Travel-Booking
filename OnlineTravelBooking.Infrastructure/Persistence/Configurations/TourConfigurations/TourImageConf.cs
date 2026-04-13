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
    public class TourImageConf : IEntityTypeConfiguration<TourImage>
    {
        public void Configure(EntityTypeBuilder<TourImage> builder)
        {
            builder.Property(ti => ti.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);
            builder.HasOne(ti=>ti.Tour)
                   .WithMany(t=>t.Images)
                   .HasForeignKey(ti=>ti.TourId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
