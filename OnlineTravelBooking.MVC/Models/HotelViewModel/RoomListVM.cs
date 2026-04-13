using OnlineTravelBooking.Domain.Common.Enums.HotelEnum;
using OnlineTravelBooking.Domain.Entities.HotelEntity;

namespace OnlineTravelBooking.MVC.Models.HotelViewModel
{
	public class RoomListVM
	{
		public int RoomNum { get; set; }
		public int HotelId { get; set; }

		public RoomStatus roomStatus { get; set; } = RoomStatus.SeaView;
		public RoomType roomType { get; set; } = RoomType.doubleRoom;
		public RoomAvailability roomAvailability { get; set; } = RoomAvailability.Available;

		public decimal BasePricePerNight { get; set; }
		public int MaxPeople { get; set; }
		public string Currency { get; set; } = "EGP";
		public string? Description { get; set; } = "Standard room";
		public bool Refundable { get; set; } = false;


	}
}
