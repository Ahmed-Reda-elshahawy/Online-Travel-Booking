using OnlineTravelBooking.Application.DTOs;
using NetTopologySuite.Geometries;
using OnlineTravelBooking.Domain.Entities.HotelEntity;


namespace OnlineTravelBooking.MVC.Models.HotelViewModel
{
	public class HotelListVM
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;  // اسم الفندق
		public string Description { get; set; } = null!;  // وصف الفندق
		public string Address { get; set; } = null!;  // العنوان
		public string City { get; set; } = null!;  // المدينة
		public string Country { get; set; } = null!;  // البلد
		public string PostalCode { get; set; } = null!;  // الرمز البريدي

		// الموقع كنقط Latitude و Longitude
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }


		public int StarRating { get; set; } 

		public TimeSpan CheckInTime { get; set; } // 
		public TimeSpan CheckOutTime { get; set; } // وقت الخروج

		public string ContactPhone { get; set; } = null!;  // رقم الهاتف للتواصل
		public string? WebsiteURL { get; set; } // رابط الموقع الإلكتروني (اختياري)

		public IList<RoomListVM> roomList { get; set; } = new List<RoomListVM>();
	}
}
