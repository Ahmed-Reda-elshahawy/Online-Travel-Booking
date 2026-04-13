using OnlineTravelBooking.Domain.Entities.Base;
using OnlineTravelBooking.Domain.Entities.HotelEntity;
using OnlineTravelBooking.Domain.Entities.User;

namespace OnlineTravelBooking.Domain.Entities.Booking;

public class BookingRoom 
{
	public int Id { get; set; }
	public int RoomId { get; set; }
	public int BookingId { get; set; }
	public DateTime CheckInDate { get; set; }
	public int Nights { get; set; }
	public int people { get; set; }
	public decimal Total { get; set; }
	public string Currency {  get; set; }
	public int UserId { get; set; }

	public ApplicationUser User { get; set; }
	public Room room { get; set; }
	public BaseBooking booking { get; set; }

}
