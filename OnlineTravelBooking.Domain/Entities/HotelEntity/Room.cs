using OnlineTravelBooking.Domain.Common.Enums.HotelEnum;
using OnlineTravelBooking.Domain.Entities.Base;

namespace OnlineTravelBooking.Domain.Entities.HotelEntity;

public class Room : BaseEntity
{
	public int RoomNum { get; set; }
	public int HotelId { get; set; }
	public string Description { get; set; }
	public int MaxPeople { get; set; }
	public decimal BasePricePerNight { get; set; }
	public string Currency {  get; set; }
	public bool Refundable { get; set; }
	public RoomStatus Status { get; set; }
	public RoomType RoomType { get; set; }
	public RoomAvailability availability { get; set; }
	public Hotel hotel { get; set; }
}
