using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineTravelBooking.Domain.Entities.Booking;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

public class BookingRoomConfig : IEntityTypeConfiguration<BookingRoom>
{
	public void Configure(EntityTypeBuilder<BookingRoom> builder)
	{
		builder.HasKey(br => new { br.BookingId, br.RoomId });
	}
}
