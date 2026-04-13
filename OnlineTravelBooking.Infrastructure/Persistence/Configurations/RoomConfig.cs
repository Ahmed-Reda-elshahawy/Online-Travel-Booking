using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.Infrastructure.Persistence.Configurations;

public class RoomConfig : IEntityTypeConfiguration<Room>
{
	

	public void Configure(EntityTypeBuilder<Room> builder)
	{
		builder.HasOne(r => r.hotel)
			.WithMany().HasForeignKey(r => r.HotelId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
	//public void Configure(EntityTypeBuilder<Booking> builder)
	//{
	//	builder.HasOne(b => b.HotelId
	//		.WithMany(p => p.Rooms)
	//		.HasForeignKey(b => b.PatientId)
	//		.OnDelete(DeleteBehavior.Restrict);

		//builder.HasOne(b => b.Doctor)
		//	.WithMany(d => d.Bookings)
		//	.HasForeignKey(b => b.DoctorId)
		//	.OnDelete(DeleteBehavior.Restrict);

		//builder.HasOne(b => b.AvailabilitySlot)
		//	.WithMany()
		//	.HasForeignKey(b => b.AvailabilitySlotId)
		//	.OnDelete(DeleteBehavior.Restrict);