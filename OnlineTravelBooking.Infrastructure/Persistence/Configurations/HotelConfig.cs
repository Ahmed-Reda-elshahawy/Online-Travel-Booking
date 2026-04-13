
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

public class HotelConfig : IEntityTypeConfiguration<Hotel>
{
	public void Configure(EntityTypeBuilder<Hotel> builder)
	{
		builder.Property(h => h.Location)
	   .HasColumnType("geography")
	   .IsRequired(false); // لازم يسمح بالـ null

	}
}
