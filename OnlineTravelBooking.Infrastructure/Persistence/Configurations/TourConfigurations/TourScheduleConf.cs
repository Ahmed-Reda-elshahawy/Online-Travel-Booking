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
    public class TourScheduleConf : IEntityTypeConfiguration<TourSchedule>
    {
        public void Configure(EntityTypeBuilder<TourSchedule> builder)
        {
            builder.HasOne(ts => ts.Tour)
                   .WithMany(t => t.Schedules)
                   .HasForeignKey(ts => ts.TourId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
