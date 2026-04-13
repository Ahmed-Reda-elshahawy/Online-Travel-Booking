using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

internal class RoomImageConfig : IEntityTypeConfiguration<RoomImage>
{
	public void Configure(EntityTypeBuilder<RoomImage> builder)
	{
		builder.HasOne(x => x.Room)
			.WithMany()
			.HasForeignKey(x => x.RoomId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
