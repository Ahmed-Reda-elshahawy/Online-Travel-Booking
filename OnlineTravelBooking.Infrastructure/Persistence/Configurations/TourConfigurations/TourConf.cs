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
    public class TourConf : IEntityTypeConfiguration<Tour>
    {
        public void Configure(EntityTypeBuilder<Tour> builder)
        {
            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(t => t.Category)
                   .WithMany()
                   .HasForeignKey(t => t.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
