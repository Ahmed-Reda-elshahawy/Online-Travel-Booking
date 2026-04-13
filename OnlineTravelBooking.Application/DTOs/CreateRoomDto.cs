
using OnlineTravelBooking.Domain.Common.Enums.HotelEnum;

namespace OnlineTravelBooking.Application.DTOs;

public class CreateRoomDto
{
	public int RoomNum { get; set; }
	public int HotelId { get; set; }

	public RoomStatus roomStatus { get; set; }
	public RoomType roomType { get; set; }
	public RoomAvailability roomAvailability { get; set; }

	public decimal BasePricePerNight { get; set; }
	public int MaxPeople { get; set; }
}
