
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

public class HotelImageconfig : IEntityTypeConfiguration<HotelImage>
{
	public void Configure(EntityTypeBuilder<HotelImage> builder)
	{
		builder.HasOne(x=>x.hotel)
			.WithMany()
			.HasForeignKey(x => x.HotelId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
